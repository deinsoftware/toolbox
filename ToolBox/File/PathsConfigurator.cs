using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using ToolBox.Platform;

namespace ToolBox.Files
{
    public sealed class PathsConfigurator
    {
        static ICommandSystem _commandSystem { get; set; }
        static IFileSystem _fileSystem { get; set; }

        public PathsConfigurator(ICommandSystem commandSystem, IFileSystem fileSystem)
        {
            if (commandSystem == null)
            {
                throw new ArgumentException(nameof(commandSystem));
            }

            if (fileSystem == null)
            {
                throw new ArgumentException(nameof(fileSystem));
            }

            _commandSystem = commandSystem;
            _fileSystem = fileSystem;
        }

        public string Combine(params string[] paths)
        {
            string path = _fileSystem.PathCombine(paths);
            path = _commandSystem.GetHomeFolder(path);
            return path;
        }

        public string Split(string fullPath, string mainPath)
        {
            if (String.IsNullOrEmpty(fullPath))
            {
                throw new ArgumentException(nameof(fullPath));
            }

            if (String.IsNullOrEmpty(mainPath))
            {
                throw new ArgumentException(nameof(mainPath));
            }

            string path = "";
            if (fullPath != mainPath)
            {
                path = fullPath.Replace(mainPath, "").Substring(1);
            }
            return path;
        }

        public string GetFileName(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            return _fileSystem.GetFileName(filePath);
        }

        public List<string> GetFiles(string path, string filter = null, SearchOption search = SearchOption.TopDirectoryOnly)
        {
            if (!_fileSystem.DirectoryExists(path))
            {
                throw new DirectoryNotFoundException();
            }

            List<string> files = new List<string>(_fileSystem.GetFiles(path, filter, search).OrderBy(name => name));
            return files;
        }

        public string GetDirectoryName(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            return _fileSystem.GetDirectoryName(path);
        }

        public List<string> GetDirectories(string path, string filter = null)
        {
            if (!_fileSystem.DirectoryExists(path))
            {
                throw new DirectoryNotFoundException();
            }

            List<string> list = new List<string>(_fileSystem.GetDirectories(path, filter).OrderBy(name => name));
            return list;
        }
    }
}

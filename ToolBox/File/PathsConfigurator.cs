using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using ToolBox.Files;
using ToolBox.Platform;

namespace ToolBox.Files
{
    public sealed class PathsConfigurator
    {
        static ICommandSystem _commandSystem;
        static IFileSystem _fileSystem;

        public PathsConfigurator(ICommandSystem commandSystem, IFileSystem fileSystem)
        {
            if (commandSystem == null)
            {
                throw new ArgumentNullException(nameof(commandSystem));
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
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

        public string GetFileName(string filePath)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath)){
                    throw new ArgumentException(nameof(filePath));
                }

                return _fileSystem.GetFileName(filePath);
            }
            catch (Exception){
                throw;
            }
        }

        public List<string> GetFiles(string path, string filter = null)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(path)){
                    throw new DirectoryNotFoundException();
                }

                List<string> files = new List<string>();
                files = new List<string>(_fileSystem.GetFiles(path, filter).OrderBy(name => name));
                return files;
            }
            catch (Exception){
                throw;
            }
        }

        public string GetDirectoryName(string path)
        {
            try
            {
                if (String.IsNullOrEmpty(path)){
                    throw new ArgumentException(nameof(path));
                }

                return _fileSystem.GetDirectoryName(path);
            }
            catch (Exception){
                throw;
            }
        }
        
        public List<string> GetDirectories(string path, string filter = null)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(path)){
                    throw new DirectoryNotFoundException();
                }

                List<string> list = new List<string>();
                list = new List<string>(_fileSystem.GetDirectories(path, filter).OrderBy(name => name));
                return list;
            }
            catch (Exception){
                throw;
            }
        }
    }
}

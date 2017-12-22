using System.Collections.Generic;
using System.IO;

namespace ToolBox.Files
{
    public static class FileSystem
    {
        public static IFileSystem Default
        {
            get
            {
                return new DefaultFileSystem();
            }
        }
    }

    public sealed class DefaultFileSystem : IFileSystem
    {
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public void MoveFile(string filePath, string newFilePath)
        {
            File.Move(filePath, newFilePath);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetPathRoot(string path)
        {
            return Path.GetPathRoot(path);
        }

        public IEnumerable<string> GetDirectories(string path, string filter)
        {
            return Directory.EnumerateDirectories(path, (filter ?? "*"));
        }

        public IEnumerable<string> GetFiles(string path, string filter)
        {
            return Directory.EnumerateFiles(path, (filter ?? "*"));
        }
    }
}
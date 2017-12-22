using System.Collections.Generic;

namespace ToolBox.Files
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        string GetFullPath(string path);
        string GetFileName(string filePath);
        string GetPathRoot(string path);
        string GetDirectoryName(string path);
        string PathCombine(params string[] paths);
        void MoveFile(string filePath, string newFilePath);
        IEnumerable<string> GetDirectories(string path, string filter);
        IEnumerable<string> GetFiles(string path, string filter);
    }
}
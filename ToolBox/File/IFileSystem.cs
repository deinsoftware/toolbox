using System.Collections.Generic;
using System.IO;

namespace ToolBox.Files
{
    public interface IFileSystem
    {
        string GetPathRoot(string path);
        string GetFullPath(string path);
        string PathCombine(params string[] paths);

        string GetFileName(string filePath);
        bool FileExists(string path);
        FileStream FileCreate(string path);
        StreamWriter FileAppendText(string text);
        void CopyFile(string filePath, string newFilePath, bool overwrite = false);
        void MoveFile(string filePath, string newFilePath);
        void DeleteFile(string filePath);
        IEnumerable<string> GetFiles(string path, string filter, SearchOption searchOption);

        string GetDirectoryName(string path);
        bool DirectoryExists(string path);
        void DeleteDirectory(string path, bool recursive = true);
        void CreateDirectory(string path);
        IEnumerable<string> GetDirectories(string path, string filter);
        IEnumerable<string> GetDirectories(string path, string filter, SearchOption searchOption);
    }
}
namespace ToolBox.System
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        string GetFullPath(string path);
        string GetFileName(string filePath);
        string GetPathRoot(string path);
        string GetDirectoryName(string path);
        string PathCombine(string firstPath, string secondPath);
        void MoveFile(string filePath, string newFilePath);
    }
}
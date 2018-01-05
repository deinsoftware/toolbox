using System;
using System.IO;
using ToolBox.Files;

namespace ToolBox.Log
{
    public class FileLogTxt : ILogSystem
    {
        static IFileSystem _fileSystem {get; set;}
        static string _logFile {get; set;}

        public FileLogTxt(IFileSystem fileSystem, string path, string fileName)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            _fileSystem = fileSystem;
            _logFile = _fileSystem.PathCombine(path, $"{fileName}.txt");
            AccessValidation();
        }

        public void AccessValidation()
        {
            try
            {
                if (!_fileSystem.FileExists(_logFile))
                {
                    _fileSystem.FileCreate(_logFile).Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save(Exception ex, LogLevel logLevel = LogLevel.Information)
        {
            try
            {
                using (StreamWriter sw = _fileSystem.FileAppendText(_logFile))
                {
                    sw.WriteLine();
                    sw.WriteLine($"EXCEPTION");
                    sw.WriteLine($"Date/Time:     {DateTime.Now}");
                    sw.WriteLine($"Level:         {logLevel.ToString()}");
                    sw.WriteLine($"Error Message: {ex.Message}");
                    sw.WriteLine($"Stack Trace:   {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        sw.WriteLine();
                        sw.WriteLine($"INNER EXCEPTION");
                        sw.WriteLine($"Error Message: {ex.InnerException.Message}");
                        sw.WriteLine($"Stack Trace:   {ex.InnerException.StackTrace}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
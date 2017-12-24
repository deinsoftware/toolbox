using System;
using System.IO;
using System.Text;
using ToolBox.Files;

namespace ToolBox.Log
{
    public class FileLogCsv : ILog
    {
        static IFileSystem _fileSystem;
        static string _logFile;
        static char _logDelimiter;

        public FileLogCsv(IFileSystem fileSystem, string path, string fileName, char delimiter = ',')
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
            _logFile = _fileSystem.PathCombine(path, fileName + ".csv");
            _logDelimiter = delimiter;

            AccessValidation();
        }

        public void AccessValidation()
        {
            try
            {
                if (!_fileSystem.FileExists(_logFile))
                {
                    _fileSystem.FileCreate(_logFile).Dispose();
                    AddHeaders();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void AddHeaders()
        {
            try
            {
                var header = new StringBuilder();
                header.Append($"Date/Time{_logDelimiter}");
                header.Append($"Level{_logDelimiter}");
                header.Append($"Error Message{_logDelimiter}");
                header.Append($"Stack Trace{_logDelimiter}");
                header.Append($"Inner Error Message{_logDelimiter}");
                header.Append($"Inner Stack Trace{_logDelimiter}");
                using (StreamWriter sw = _fileSystem.FileAppendText(_logFile))
                {
                    sw.WriteLine(header);
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
                var exception = new StringBuilder();
                exception.Append($"{DateTime.Now}{_logDelimiter}");
                exception.Append($"{logLevel.ToString()}{_logDelimiter}");
                exception.Append($"{ex.Message}{_logDelimiter}");
                exception.Append($"{ex.StackTrace}{_logDelimiter}");
                exception.Append($"{ex.InnerException?.Message ?? ""}{_logDelimiter}");
                exception.Append($"{ex.InnerException?.StackTrace ?? ""}{_logDelimiter}");

                using (StreamWriter sw = _fileSystem.FileAppendText(_logFile))
                {
                    sw.WriteLine(exception);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

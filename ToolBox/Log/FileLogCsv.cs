using System;
using System.IO;
using System.Text;
using ToolBox.Files;

namespace ToolBox.Log
{
    public class FileLogCsv : ILogSystem
    {
        static IFileSystem _fileSystem { get; set; }
        static string _logFile { get; set; }
        static char _logDelimiter { get; set; }

        public FileLogCsv(IFileSystem fileSystem, string path, string fileName, char delimiter = ',')
        {
            if (fileSystem == null)
            {
                throw new ArgumentException(nameof(fileSystem));
            }

            if (path == null)
            {
                throw new ArgumentException(nameof(path));
            }

            if (fileName == null)
            {
                throw new ArgumentException(nameof(fileName));
            }

            _fileSystem = fileSystem;
            _logFile = _fileSystem.PathCombine(path, $"{fileName}.csv");
            _logDelimiter = delimiter;

            AccessValidation();
        }

        public void AccessValidation()
        {
            if (!_fileSystem.FileExists(_logFile))
            {
                _fileSystem.FileCreate(_logFile).Dispose();
                AddHeaders();
            }
        }

        void AddHeaders()
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

        public void Save(Exception ex, LogLevel logLevel = LogLevel.Information)
        {
            var message = new StringBuilder();
            message.Append($"{DateTime.Now}{_logDelimiter}");
            message.Append($"{logLevel.ToString()}{_logDelimiter}");
            message.Append($"{ex.Message}{_logDelimiter}");
            message.Append($"{ex.StackTrace}{_logDelimiter}");
            message.Append($"{ex.InnerException?.Message ?? ""}{_logDelimiter}");
            message.Append($"{ex.InnerException?.StackTrace ?? ""}{_logDelimiter}");

            using (StreamWriter sw = _fileSystem.FileAppendText(_logFile))
            {
                sw.WriteLine(message);
            }
        }
    }
}

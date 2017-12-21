using System;
using System.IO;
using System.Text;
using ToolBox.System;

namespace ToolBox.Log
{
    public class LogFileCsv : ILog
    {
        private string _logFile;
        private char _logDelimiter;

        public LogFileCsv(string path, string fileName, char delimiter = ',')
        {
            _logFile = Path.Combine(path, fileName + ".csv");
            _logDelimiter = delimiter;

            AccessValidation();
        }

        public void AccessValidation()
        {
            try
            {
                if (!File.Exists(_logFile))
                {
                    File.Create(_logFile).Dispose();
                    AddHeaders();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddHeaders()
        {
            try
            {
                var header = new StringBuilder();
                header.Append("Date/Time");             header.Append(_logDelimiter);
                header.Append("Error Message");         header.Append(_logDelimiter);
                header.Append("Stack Trace");           header.Append(_logDelimiter);
                header.Append("Inner Error Message");   header.Append(_logDelimiter);
                header.Append("Inner Stack Trace");     header.Append(_logDelimiter);
                using (StreamWriter sw = File.AppendText(_logFile))
                {
                    sw.WriteLine(header.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save(Exception ex)
        {
            try
            {
                var exception = new StringBuilder();
                exception.Append(DateTime.Now);                         exception.Append(_logDelimiter);
                exception.Append(ex.Message);                           exception.Append(_logDelimiter);
                exception.Append(ex.StackTrace);                        exception.Append(_logDelimiter);
                exception.Append(ex.InnerException?.Message ?? "");     exception.Append(_logDelimiter);
                exception.Append(ex.InnerException?.StackTrace ?? "");  exception.Append(_logDelimiter);

                using (StreamWriter sw = File.AppendText(_logFile))
                {
                    sw.WriteLine(exception.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


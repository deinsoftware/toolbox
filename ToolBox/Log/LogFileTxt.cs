using System;
using System.IO;
using ToolBox.System;

namespace ToolBox.Log
{
    public class LogFileTxt : ILog
    {
        private string _logFile;

        public LogFileTxt(string path, string fileName)
        {
            _logFile = Path.Combine(path, fileName + ".txt");
            AccessValidation();
        }

        public void AccessValidation()
        {
            try
            {
                if (!File.Exists(_logFile))
                {
                    File.Create(_logFile).Dispose();
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
                using (StreamWriter sw = File.AppendText(_logFile))
                {
                    sw.WriteLine();
                    sw.WriteLine("EXCEPTION");
                    sw.WriteLine("Date/Time:     " + DateTime.Now);
                    sw.WriteLine("Error Message: " + ex.Message);
                    sw.WriteLine("Stack Trace:   " + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        sw.WriteLine();
                        sw.WriteLine("INNER EXCEPTION");
                        sw.WriteLine("Error Message: " + ex.InnerException.Message);
                        sw.WriteLine("Stack Trace:   " + ex.InnerException.StackTrace);
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
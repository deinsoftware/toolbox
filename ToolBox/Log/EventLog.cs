using System;
using System.IO;
using System.Text;
using ToolBox.Files;

namespace ToolBox.Log
{
    public class EventLogTxt : ILog
    {
        static string _applicationName;

        public EventLogTxt(string applicationName)
        {
            if (applicationName == null)
            {
                throw new ArgumentNullException(nameof(applicationName));
            }

            _applicationName = applicationName;
            AccessValidation();
        }

        public void AccessValidation()
        {
            try
            {
                //TODO: Check EventLog permission
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
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = _applicationName;
                    var message = new StringBuilder();
                    message.Append($"EXCEPTION                                              {Environment.NewLine}");
                    message.Append($"Date/Time:     {DateTime.Now}                          {Environment.NewLine}");
                    message.Append($"Error Message: {ex.Message}                            {Environment.NewLine}");
                    message.Append($"Stack Trace:   {ex.StackTrace}                         {Environment.NewLine}");
                    if (ex.InnerException != null)
                    {
                        message.Append($"                                                   {Environment.NewLine}");
                        message.Append($"INNER EXCEPTION                                    {Environment.NewLine}");
                        message.Append($"Error Message: {ex.InnerException.Message}         {Environment.NewLine}");
                        message.Append($"Stack Trace:   {ex.InnerException.StackTrace}      {Environment.NewLine}");
                    }
                    eventLog.WriteEntry(message, logLevel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
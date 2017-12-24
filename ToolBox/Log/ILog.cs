using System;

namespace ToolBox.Log
{
    public interface ILog
    {
        void AccessValidation();
        void Save(Exception ex, LogLevel logLevel);
    }

    public enum LogLevel
    {
        Error = 1,
        Warning = 2,
        Information = 4,
        SuccessAudit = 8,
        FailureAudit = 16
    }
}
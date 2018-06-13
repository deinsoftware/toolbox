using System;

namespace ToolBox.Log
{
    public interface ILogSystem

    {
        void AccessValidation();
        void Save(Exception ex, LogLevel logLevel = LogLevel.Information);
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
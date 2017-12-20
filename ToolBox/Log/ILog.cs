using System;

namespace ToolBox.Log
{
    public interface ILog
    {
        void AccessValidation();
        void Save(Exception ex);
    }
}
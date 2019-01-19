using System;
using System.IO;
using System.Reflection;

namespace ToolBox.Bridge
{
    public static class Utility
    {
        public static string GetExecutionAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}

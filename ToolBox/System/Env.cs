using System;

namespace ToolBox.System
{
    public static class Env
    {
        public static string GetValue(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public static void SetValue(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);
        }

        public static bool IsNullOrEmpty(string name)
        {
            string env = Env.GetValue(name);
            return String.IsNullOrEmpty(env);
        }
    }
}

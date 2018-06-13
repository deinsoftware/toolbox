using System;

namespace ToolBox.System
{
    public static class User
    {

        public static string GetUserName()
        {
            return Env.GetValue("USERNAME") ?? Env.GetValue("USER");
        }

        public static string GetMachine()
        {
            return Environment.MachineName;
        }

        public static string GetDomain()
        {
            return Env.GetValue("USERDOMAIN") ?? Env.GetValue("HOSTNAME");
        }
    }
}

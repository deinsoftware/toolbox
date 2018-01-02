using System;

namespace ToolBox.System
{
    public static class User
    {

        public static string GetUserName(){
            try
            {
                return Env.GetValue("USERNAME") ?? Env.GetValue("USER");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetMachine(){
            try
            {
                return Environment.MachineName;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public static string GetDomain(){
            try
            {
                return Env.GetValue("USERDOMAIN") ?? Env.GetValue("HOSTNAME");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.System
{
    public static class Env
    {
        public static string GetValue(string name) {
            try
            {
                return Environment.GetEnvironmentVariable(name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SetValue(string name, string value) {
            try
            {
                Environment.SetEnvironmentVariable(name, value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsNullOrEmpty(string name){
            try
            {
                string env = Env.GetValue(name);
                return String.IsNullOrEmpty(env);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

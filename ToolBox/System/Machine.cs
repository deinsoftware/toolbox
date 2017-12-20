using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.System
{
    public static class Machine
    {

        public static string GetUser(){
            try
            {
                return Env.Get("USERNAME") ?? Env.Get("USER");
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public static string GetDomain(){
            try
            {
                return Env.Get("USERDOMAIN") ?? Env.Get("HOSTNAME");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

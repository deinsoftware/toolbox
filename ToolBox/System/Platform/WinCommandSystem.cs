using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.System
{
    public static partial class CommandSystem
    {
        public static ICommandSystem Win
        {
            get
            {
                return new WinCommandSystem();
            }
        }
    }

    public sealed class WinCommandSystem : ICommandSystem
    {
        public string PathNormalizer(string path) {
            try
            {
                return path.Replace(@"/",@"\");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetUserFolder(string path) {
            try
            {
                return PathNormalizer(path.Replace("~",$"{Env.GetValue("USERPROFILE")}"));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

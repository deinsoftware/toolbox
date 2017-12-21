using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.System.Command
{
    public class WinCommand : ICommand
    {
        public string PathNormalizer(string path) {
            try
            {
                return path.ToBackslash();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetUserFolder(string path) {
            try
            {
                return path.Replace("~",$"{Env.GetValue("USERPROFILE")}").ToBackslash();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

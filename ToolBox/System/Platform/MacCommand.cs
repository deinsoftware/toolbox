using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.System.Command
{
    public class MacCommand : ICommand
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
                return path.Replace("~",$"/Users/{Machine.GetUser()}").ToBackslash();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

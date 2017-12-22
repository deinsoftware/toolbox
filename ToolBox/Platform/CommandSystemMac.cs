using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using ToolBox.System;

namespace ToolBox.Platform
{
    public static partial class CommandSystem
    {
        public static ICommandSystem Mac
        {
            get
            {
                return new CommandSystemMac();
            }
        }
    }
    public sealed class CommandSystemMac : ICommandSystem
    {
        public string PathNormalizer(string path) {
            try
            {
                return path.Replace(@"\",@"/");
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public string GetHomeFolder(string path) {
            try
            {
                return path.Replace("~",$"/Users/{Machine.GetUser()}").Replace(@"\",@"/");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

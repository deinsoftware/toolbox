using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.System
{
    public static partial class CommandSystem
    {
        public static ICommandSystem Mac
        {
            get
            {
                return new MacCommandSystem();
            }
        }
    }
    public sealed class MacCommandSystem : ICommandSystem
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
        
        public string GetUserFolder(string path) {
            try
            {
                return PathNormalizer(path.Replace("~",$"/Users/{Machine.GetUser()}"));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

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
        public string PathNormalizer(string path)
        {
            return path.Replace(@"\", @"/");
        }

        public string GetHomeFolder(string path)
        {
            return path.Replace("~", $"/Users/{User.GetUserName()}").Replace(@"\", @"/");
        }
    }
}

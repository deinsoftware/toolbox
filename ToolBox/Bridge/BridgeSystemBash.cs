using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using ToolBox.System;
using System.IO;
using System.Diagnostics;

namespace ToolBox.Bridge
{
    public static partial class BridgeSystem
    {
        public static IBridgeSystem Bash
        {
            get
            {
                return new BridgeSystemBash();
            }
        }
    }

    public sealed class BridgeSystemBash : IBridgeSystem
    {
        public string GetFileName()
        {
            return "/bin/bash";
        }

        public string CommandConstructor(string command, Output? output = Output.Hidden, string dir = "")
        {
            if (!String.IsNullOrEmpty(dir))
            {
                dir = $" '{dir}'";
            }
            if (output == Output.External)
            {
                command = $"sh {Directory.GetCurrentDirectory()}/cmd.sh '{command}'{dir}";
            }
            command = $"-c \"{command}\"";
            return command;
        }

        public void Browse(string url)
        {
            Process.Start("open", url);
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using ToolBox.System;
using System.Diagnostics;
using System.IO;

namespace ToolBox.Bridge
{
    public static partial class BridgeSystem
    {
        public static IBridgeSystem Bat
        {
            get
            {
                return new BridgeSystemBat();
            }
        }
    }

    public sealed class BridgeSystemBat : IBridgeSystem
    {
        public string GetFileName()
        {
            return "cmd.exe";
        }

        public string CommandConstructor(string command, Output? output = Output.Hidden, string dir = "")
        {
            if (!String.IsNullOrEmpty(dir))
            {
                dir = $" \"{dir}\"";
            }
            if (output == Output.External)
            {
                command = $"{Directory.GetCurrentDirectory()}/cmd.bat \"{command}\"{dir}";
            }
            command = $"/c \"{command}\"";
            return command;
        }

        public void Browse(string url)
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}

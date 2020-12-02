using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

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

        public string[] CommandConstructor(string command, Output? output = Output.Hidden, string dir = "")
        {
            List<string> list = new List<string>();

            if (output == Output.External)
            {
                list.Add($"{@Directory.GetCurrentDirectory()}/cmd.bat");
                list.Add($"{command}");
                if (!String.IsNullOrEmpty(dir))
                {
                    list.Add($"{@dir}");
                }
            }
            else
            {
                list.Add($"/c");
                list.Add($"{command}");
            }
            return list.ToArray();
        }

        public void Browse(string url)
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}

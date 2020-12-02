using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

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

        public string[] CommandConstructor(string command, Output? output = Output.Hidden, string dir = "")
        {
            List<string> list = new List<string>();

            if (output == Output.External)
            {
                list.Add($"{@Directory.GetCurrentDirectory()}/cmd.sh");
                list.Add($"{command}");
                if (!String.IsNullOrEmpty(dir))
                {
                    list.Add($"{@dir}");
                }
            }
            else
            {
                list.Add($"-c");
                list.Add($"{command}");
            }
            return list.ToArray();
        }

        public void Browse(string url)
        {
            Process.Start("open", url);
        }
    }
}

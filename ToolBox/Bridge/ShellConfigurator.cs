using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ToolBox.Notification;
using ToolBox.Platform;
using ToolBox.Transform;

namespace ToolBox.Bridge
{
    public sealed class ShellConfigurator
    {
        static IBridgeSystem _bridgeSystem { get; set; }
        static INotificationSystem _notificationSystem { get; set; }

        public ShellConfigurator(IBridgeSystem bridgeSystem, INotificationSystem notificationSystem = null)
        {
            if (bridgeSystem == null)
            {
                throw new ArgumentException(nameof(bridgeSystem));
            }
            _bridgeSystem = bridgeSystem;

            if (notificationSystem == null)
            {
                _notificationSystem = NotificationSystem.Default;
            }
            else
            {
                _notificationSystem = notificationSystem;
            }

            if (!OS.IsWin())
            {
                Term("chmod +x cmd.sh");
            }
        }

        public void Browse(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                _bridgeSystem.Browse(url);
            }
        }

        public Response Term(string command, Output? output = Output.Hidden, string dir = "")
        {
            var result = new Response();
            var stderr = new StringBuilder();
            var stdout = new StringBuilder();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _bridgeSystem.GetFileName();
            startInfo.Arguments = _bridgeSystem.CommandConstructor(command, output, dir);
            startInfo.RedirectStandardInput = false;
            startInfo.RedirectStandardOutput = (output != Output.External);
            startInfo.RedirectStandardError = (output != Output.External);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = (output != Output.External);
            if (!String.IsNullOrEmpty(dir) && output != Output.External)
            {
                startInfo.WorkingDirectory = dir;
            }

            using (Process process = Process.Start(startInfo))
            {
                switch (output)
                {
                    case Output.Internal:
                        _notificationSystem.StandardLine();

                        while (!process.StandardOutput.EndOfStream)
                        {
                            string line = process.StandardOutput.ReadLine();
                            stdout.AppendLine(line);
                            _notificationSystem.StandardOutput(line);
                        }

                        while (!process.StandardError.EndOfStream)
                        {
                            string line = process.StandardError.ReadLine();
                            stderr.AppendLine(line);
                            _notificationSystem.StandardError(line);
                        }
                        break;
                    case Output.Hidden:
                        stdout.AppendLine(process.StandardOutput.ReadToEnd());
                        stderr.AppendLine(process.StandardError.ReadToEnd());
                        break;
                }

                process.WaitForExit();
                result.stdout = stdout.ToString();
                result.stderr = stderr.ToString();
                result.code = process.ExitCode;
            }

            return result;
        }

        public void Result(string response)
        {
            response = response
                .Replace("\r", "")
                .Replace("\n", "");
            if (!String.IsNullOrEmpty(response))
            {
                _notificationSystem.StandardOutput(response);
            }
            else
            {
                _notificationSystem.StandardWarning("is not installed");
            }
        }

        public string GetWord(string request, int wordPosition)
        {
            string response = "";
            string[] stringSeparators = new string[] { " " };
            string[] words = request.Split(stringSeparators, StringSplitOptions.None);
            if (wordPosition <= words.Length)
            {
                response = words[wordPosition];
            }
            return response;
        }

        public string[] SplitLines(string request)
        {
            string[] stringSeparators = new string[] { Environment.NewLine, "\n" };
            string[] lines = request.Split(stringSeparators, StringSplitOptions.None);
            string[] response = lines;
            return response;
        }

        public string ExtractLine(string request, string search, params string[] remove)
        {
            string response = "";
            string[] stringSeparators = new string[] { Environment.NewLine, "\n" };
            string[] lines = request.Split(stringSeparators, StringSplitOptions.None);
            foreach (string l in lines)
            {
                if (l.Contains(search))
                {
                    response = Strings.RemoveWords(l, remove);
                    break;
                }
            }
            return response;
        }
    }
}
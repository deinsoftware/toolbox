using System;
using Colorify;
using static Colorify.Colors;
using Colorify.UI;
using ToolBox.Bridge;
using ToolBox.Files;
using ToolBox.Notification;
using ToolBox.Platform;
using System.Collections.Generic;

namespace Sample
{
    static class Program
    {
        private static Format _colorify { get; set; }
        public static INotificationSystem _notificationSystem { get; set; }
        public static IBridgeSystem _bridgeSystem { get; set; }
        public static ShellConfigurator _shell { get; set; }

        public static DiskConfigurator _disk { get; set; }
        public static PathsConfigurator _path { get; set; }

        static void Main(string[] args)
        {
            try
            {
                _disk = new DiskConfigurator(FileSystem.Default);
                switch (OS.GetCurrent())
                {
                    case "win":
                        _path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                        break;
                    case "mac":
                        _path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                        break;
                }

                _notificationSystem = NotificationSystem.Default;
                switch (OS.GetCurrent())
                {
                    case "win":
                        _bridgeSystem = BridgeSystem.Bat;
                        _colorify = new Format(Theme.Dark);
                        break;
                    case "gnu":
                        _bridgeSystem = BridgeSystem.Bash;
                        _colorify = new Format(Theme.Dark);
                        break;
                    case "mac":
                        _bridgeSystem = BridgeSystem.Bash;
                        _colorify = new Format(Theme.Light);
                        break;
                }
                _shell = new ShellConfigurator(_bridgeSystem, _notificationSystem);
                Menu();
                _colorify.ResetColor();
                _colorify.Clear();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageException("Ahh my eyes! Why this console is too small?");
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }
        }

        static void Menu()
        {
            _colorify.Clear();
            _colorify.DivisionLine('=', Colorify.Colors.bgInfo);
            _colorify.AlignCenter("SHELL", Colorify.Colors.bgInfo);
            _colorify.DivisionLine('=', Colorify.Colors.bgInfo);
            _colorify.BlankLines();
            _colorify.Write($"{" 1]",-4}"); _colorify.WriteLine("Browse");
            _colorify.Write($"{" 2]",-4}"); _colorify.WriteLine("Hidden");
            _colorify.Write($"{" 3]",-4}"); _colorify.WriteLine("Internal");
            _colorify.Write($"{" 4]",-4}"); _colorify.WriteLine("External");
            _colorify.Write($"{" 5]",-4}"); _colorify.WriteLine("Special");
            _colorify.BlankLines();
            _colorify.WriteLine($"{" X] Exit",-17}", txtDanger);
            _colorify.BlankLines();
            _colorify.DivisionLine('=', Colorify.Colors.bgInfo);
            _colorify.BlankLines();
            _colorify.Write($"{" Make your choice:",-25}");
            string opt = Console.ReadLine();
            opt = opt.ToLower();

            _colorify.Clear();
            switch (opt)
            {
                case "1": Browse(); break;
                case "2": ShellHidden(); break;
                case "3": ShellInternal(); break;
                case "4": ShellExternal(); break;
                case "5": Special(); break;
                case "X": Exit(); break;
                case "x": Exit(); break;
                default: Menu(); break;
            }
        }

        static void Back()
        {
            _colorify.BlankLines();
            _colorify.Write("Press [ANY] key to continue ");
            Console.ReadKey();
            Menu();
        }

        static void MessageException(string message)
        {
            _colorify.ResetColor();
            _colorify.Clear();
            _colorify.WriteLine(message, bgDanger);
        }

        static void Shell()
        {
            try
            {
                Menu();
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }
        }

        static void Browse()
        {
            try
            {
                _shell.Browse("https://www.github.com/deinsoftware");
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }

            Menu();
        }

        static void ShellHidden()
        {
            try
            {
                Response result = _shell.Term("dotnet --version", Output.Hidden);
                _shell.Result(result.stdout, "Not Installed");
                _colorify.WriteLine(result.code.ToString(), txtInfo);
                if (result.code == 0)
                {
                    _colorify.WriteLine($"Command Works :D", txtSuccess);
                }
                else
                {
                    _colorify.WriteLine(result.stderr, txtDanger);
                }

                Back();
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }
        }

        static void ShellInternal()
        {
            try
            {
                _shell.Term("java -showversion 2>&1", Output.Internal);

                Back();
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }
        }

        static void ShellExternal()
        {
            try
            {
                Response result = _shell.Term("node -v", Output.External);

                Back();
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }
        }

        static void Special()
        {
            try
            {
                string id = "99999999";
                string name = "App Test iOS";
                string command = $"ionic cordova plugin add cordova-plugin-facebook4 --variable APP_ID='{id}' --variable APP_NAME='{name}'";

                string path = _path.Combine("~", "Folder with Spaces");

                Response result = _shell.Term(command, Output.External, path);

                Back();
            }
            catch (Exception ex)
            {
                MessageException(ex.ToString());
            }
        }

        static void Exit()
        {
            _colorify.ResetColor();
            _colorify.Clear();
        }
    }
}

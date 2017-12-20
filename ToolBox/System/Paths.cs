using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using ToolBox.System.Command;

namespace ToolBox.System
{
    public static class Paths
    {
        static ICommand _cmd;

        static Paths(){
            switch (Platform.GetCurrent())
            {
                case "win":
                    _cmd = new WinCommand();
                    break;
                case "mac":
                    _cmd = new MacCommand();
                    break;
            }
        }

        public static string ToSlash(this string path){
            return path.Replace(@"\",@"/");
        }

        public static string ToBackslash(this string path){
            return path.Replace(@"/",@"\");
        }

        public static string Combine(params string[] paths)
        {
            string path = Path.Combine(paths);
            path = _cmd.GetUserFolder(path);
            return path;
        }

        public static List<string> GetDirectories(this string path, string filter){
            try
            {
                List<string> list = new List<string>();
                list = new List<string>(Directory.EnumerateDirectories(path, filter).OrderBy(name => name));
                return list;
            }
            catch (Exception){
                throw;
            }
        }
    
        public static List<string> GetFiles(this string path, string filter){
            try
            {
                List<string> files = new List<string>();
                files = new List<string>(Directory.EnumerateFiles(path, filter).OrderBy(name => name));
                return files;
            }
            catch (Exception){
                throw;
            }
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolBox.Transform;
using ToolBox.System.Command;

namespace ToolBox.System
{
    public static class Disk
    {
        private static ICommand _cmd;

        static Disk(){
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
        
        private static bool IsFiltered(List<string> filter, string file)
        {
            try
            {
                bool valid = false;
                if (filter == null)
                {
                    valid = true;
                } else 
                {
                    valid = filter.Any(f => file.EndsWith(f, StringComparison.OrdinalIgnoreCase));
                }
                return valid;
            }
            catch (Exception){
                throw;
            }
        }

        public static void CopyAll(string sourcePath, string destinationPath, bool overWrite = false, List<string> filter = null)
        {
            try
            {
                string[] directories = Directory
                    .GetDirectories(sourcePath, "*.*", SearchOption.AllDirectories);
                Parallel.ForEach(directories, dirPath =>
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
                });

                var files = Directory
                    .GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
                    .Where(f => IsFiltered(filter, f));
                Parallel.ForEach(files, newPath =>
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), overWrite);
                });
            }
            catch (Exception){
                throw;
            }
        }

        public static void DeleteAll(string sourcePath, bool recursive)
        {
            try
            {
                Directory.Delete(sourcePath, recursive);
            }
            catch (DirectoryNotFoundException) 
            {
                //Avoid Exception when try delete files inside deleted folder.
                return;
            }
            catch (Exception){
                throw;
            }
        }
    }
}

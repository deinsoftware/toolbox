using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolBox.Platform;
using ToolBox.Transform;

namespace ToolBox.Files
{
    public static class Disk
    {
        private static ICommandSystem _cmd;

        static Disk(){
            switch (OS.GetCurrent())
            {
                case "win":
                    _cmd = new CommandSystemWin();
                    break;
                case "mac":
                    _cmd = new CommandSystemMac();
                    break;
            }
        }
        
        private static bool IsFiltered(List<string> extensionFilter, string file)
        {
            try
            {
                bool valid = false;
                if (extensionFilter == null)
                {
                    valid = true;
                } else 
                {
                    valid = extensionFilter.Any(
                        f => file.EndsWith(f, StringComparison.OrdinalIgnoreCase)
                    );
                }
                return valid;
            }
            catch (Exception){
                throw;
            }
        }

        public static void CopyAll(string sourcePath, string destinationPath, bool overWrite = false, List<string> extensionFilter = null)
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
                    .Where(f => IsFiltered(extensionFilter, f));
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

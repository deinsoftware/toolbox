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
    public sealed class DiskConfigurator
    {
        private ICommandSystem _commandSystem;

        public DiskConfigurator(ICommandSystem commandSystem){
            if (commandSystem == null)
            {
                throw new ArgumentNullException("fileSystem is required");
            }

            _commandSystem = commandSystem;
        }
        
        private bool IsFiltered(List<string> extensionFilter, string file)
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

        public void CopyAll(string sourcePath, string destinationPath, bool overWrite = false, List<string> extensionFilter = null)
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

        public void DeleteAll(string sourcePath, bool recursive)
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

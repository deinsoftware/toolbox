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
        private IFileSystem _fileSystem;

        public DiskConfigurator(ICommandSystem commandSystem, IFileSystem fileSystem){
            if (commandSystem == null)
            {
                throw new ArgumentNullException("fileSystem is required");
            }

            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem is required");
            }

            _commandSystem = commandSystem;
            _fileSystem = fileSystem;
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
                if (!_fileSystem.DirectoryExists(sourcePath)){
                    throw new DirectoryNotFoundException();
                }
                
                CopyDirectories(sourcePath, destinationPath);
                CopyFiles(sourcePath, destinationPath, overWrite, extensionFilter);
            }
            catch (Exception){
                throw;
            }
        }

        public void CopyDirectories(string sourcePath, string destinationPath)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(sourcePath)){
                    throw new DirectoryNotFoundException();
                }
                
                var directories = _fileSystem
                    .GetDirectories(sourcePath, null, SearchOption.AllDirectories);
                Parallel.ForEach(directories, dirPath =>
                {
                    var newPath = dirPath.Replace(sourcePath, destinationPath);
                    if (!_fileSystem.DirectoryExists(newPath))
                    {
                        _fileSystem.CreateDirectory(newPath);
                    }
                });
            }
            catch (Exception){
                throw;
            }
        }

        public void CopyFiles(string sourcePath, string destinationPath, bool overWrite = false, List<string> extensionFilter = null)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(sourcePath)){
                    throw new DirectoryNotFoundException();
                }
                
                var files = _fileSystem
                    .GetFiles(sourcePath, null, SearchOption.AllDirectories)
                    .Where(f => IsFiltered(extensionFilter, f));
                Parallel.ForEach(files, filePath =>
                {
                    var newFile = filePath.Replace(sourcePath, destinationPath);
                    var newPath = _fileSystem.GetDirectoryName(newFile);
                    if (!_fileSystem.DirectoryExists(newPath))
                    {
                        _fileSystem.CreateDirectory(newPath);
                    }
                    _fileSystem.CopyFile(filePath, newFile, overWrite);
                });
            }
            catch (Exception){
                throw;
            }
        }

        public void DeleteAll(string path, bool recursive)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(path)){
                    throw new DirectoryNotFoundException();
                }

                _fileSystem.DeleteDirectory(path, recursive);
            }
            catch (Exception){
                throw;
            }
        }
    }
}

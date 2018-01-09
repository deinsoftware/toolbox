using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolBox.Transform;
using System.Text.RegularExpressions;

namespace ToolBox.Files
{
    public sealed class DiskConfigurator
    {
        static IFileSystem _fileSystem {get; set;}
        static INotificationSystem _notificationSystem {get; set;}

        public DiskConfigurator(IFileSystem fileSystem, INotificationSystem notificationSystem = null){
            if (fileSystem == null)
            {
                throw new ArgumentException(nameof(fileSystem));
            }
            _fileSystem = fileSystem;

            if (notificationSystem == null){
                _notificationSystem = NotificationSystem.Default;
            } else {
                _notificationSystem = notificationSystem;
            }
        }
        
        public List<string> FilterCreator(params string[] extension){
            return FilterCreator(false, extension);
        }

        public List<string> FilterCreator(bool ignoreSystemFiles, params string[] extension){
            try
            {
                if (extension == null)
                {
                    throw new ArgumentException(nameof(extension));
                }

                List<string> filter = new List<string>();

                string extensions = string.Join(
                    "|", 
                    Array.ConvertAll(
                        extension, 
                        ext => ext.ToString().Replace(".", "")
                    )
                );
                if (!String.IsNullOrEmpty(extensions)){
                    filter.Add($"([^\\s]+(\\.(?i)({extensions}))$)");
                }
                if (ignoreSystemFiles){
                    filter.Add(@"^(?!\.).*");
                }

                return filter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool IsFiltered(List<string> regexFilter, string file)
        {
            try
            {
                bool valid = false;
                if (regexFilter == null)
                {
                    valid = true;
                } else 
                valid = regexFilter.All(
                    filter => Regex.IsMatch(file, filter)
                );
                return valid;
            }
            catch (Exception){
                throw;
            }
        }

        public void CopyAll(string sourcePath, string destinationPath, bool overWrite = false, List<string> regexFilter = null)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(sourcePath)){
                    throw new DirectoryNotFoundException();
                }
                
                CopyDirectories(sourcePath, destinationPath);
                CopyFiles(sourcePath, destinationPath, overWrite, regexFilter);
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
                        _notificationSystem.ShowAction("COPY", Strings.RemoveWords(newPath, destinationPath));
                        _fileSystem.CreateDirectory(newPath);
                    }
                });
            }
            catch (Exception){
                throw;
            }
        }

        public void CopyFiles(string sourcePath, string destinationPath, bool overWrite = false, List<string> regexFilter = null)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(sourcePath)){
                    throw new DirectoryNotFoundException();
                }
                
                var files = _fileSystem
                    .GetFiles(sourcePath, null, SearchOption.AllDirectories)
                    .Where(file => IsFiltered(regexFilter, file));
                Parallel.ForEach(files, filePath =>
                {
                    var newFile = filePath.Replace(sourcePath, destinationPath);
                    var newPath = _fileSystem.GetDirectoryName(newFile);
                    if (!_fileSystem.DirectoryExists(newPath))
                    {
                        _fileSystem.CreateDirectory(newPath);
                    }
                    _notificationSystem.ShowAction("COPY", Strings.RemoveWords(newFile, destinationPath));
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

                _notificationSystem.ShowAction("DEL", path);
                _fileSystem.DeleteDirectory(path, recursive);
            }
            catch (Exception){
                throw;
            }
        }
    }
}

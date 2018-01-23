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
                throw new ArgumentNullException(nameof(fileSystem));
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
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
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

        bool IsFiltered(List<string> regexFilter, string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            
            bool valid = false;
            if (regexFilter == null || regexFilter.Count < 1)
            {
                valid = true;
            } else {
                valid = regexFilter.All(
                    filter => Regex.IsMatch(_fileSystem.GetFileName(file), filter)
                );
            }
            return valid;
        }

        public void CopyAll(string sourcePath, string destinationPath, bool overWrite = false, List<string> regexFilter = null)
        {
            if (!_fileSystem.DirectoryExists(sourcePath)){
                throw new DirectoryNotFoundException();
            }
            
            CopyDirectories(sourcePath, destinationPath);
            CopyFiles(sourcePath, destinationPath, overWrite, regexFilter);
        }

        public void CopyDirectories(string sourcePath, string destinationPath)
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

        public void CopyFiles(string sourcePath, string destinationPath, bool overWrite = false, List<string> regexFilter = null)
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

        public void DeleteAll(string path, bool recursive)
        {
            if (!_fileSystem.DirectoryExists(path)){
                throw new DirectoryNotFoundException();
            }

            _notificationSystem.ShowAction("DEL", path);
            _fileSystem.DeleteDirectory(path, recursive);
        }
    }
}

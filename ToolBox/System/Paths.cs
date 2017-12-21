using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using ToolBox.System;

namespace ToolBox.System
{
    public class Paths
    {
        private ICommandSystem _commandSystem;
        private IFileSystem _fileSystem;

        public Paths(ICommandSystem commandSystem, IFileSystem fileSystem)
        {
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

        public string Combine(params string[] paths)
        {
            string path = Path.Combine(paths);
            path = _commandSystem.GetUserFolder(path);
            return path;
        }

        public List<string> GetDirectories(string path, string filter = null)
        {
            try
            {
                List<string> list = new List<string>();
                list = new List<string>(Directory.EnumerateDirectories(path, (filter ?? "*")).OrderBy(name => name));
                return list;
            }
            catch (Exception){
                throw;
            }
        }
    
        public List<string> GetFiles(string path, string filter = null)
        {
            try
            {
                List<string> files = new List<string>();
                files = new List<string>(Directory.EnumerateFiles(path, (filter ?? "*")).OrderBy(name => name));
                return files;
            }
            catch (Exception){
                throw;
            }
        }
    }
}

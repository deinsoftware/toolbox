using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;

namespace ToolBox.Transform
{
    public static class Strings
    {
        public static string RemoveWords(string oldValue, params string[] wordsToRemove){
            try
            {
                string result = "";
                result = oldValue;
                foreach (var word in wordsToRemove)
                {
                    if ( String.IsNullOrEmpty(word) || (word == " ") ) {
                        throw new ArgumentException("Can't remove empty or blank spaces.", nameof(wordsToRemove));
                    }
                    result = result.Replace(word, "");
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using System;

namespace ToolBox.Transform
{
    public static class Strings
    {
        public static string RemoveWords(string oldValue, params string[] wordsToRemove){
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
    }
}

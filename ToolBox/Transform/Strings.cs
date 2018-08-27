using System;

namespace ToolBox.Transform
{
    public static class Strings
    {
        public static string CleanSpecialCharacters(string value)
        {
            value = value.Replace("\r", "").Replace("\n", "");
            return value;
        }

        public static string RemoveWords(string value, params string[] wordsToRemove)
        {
            string result = "";
            result = value;
            foreach (var word in wordsToRemove)
            {
                if (String.IsNullOrEmpty(word) || (word == " "))
                {
                    throw new ArgumentException("Can't remove empty or blank spaces.", nameof(wordsToRemove));
                }
                result = result.Replace(word, "");
            }
            return result;
        }

        public static string GetWord(string value, int wordPosition)
        {
            string response = "";
            string[] stringSeparators = new string[] { " " };
            string[] words = value.Split(stringSeparators, StringSplitOptions.None);
            if (wordPosition < 0)
            {
                throw new ArgumentException("Can't be less than zero.", nameof(wordPosition));
            }
            if (wordPosition < words.Length)
            {
                response = words[wordPosition];
            }
            return response;
        }

        public static string[] SplitLines(string value)
        {
            string[] stringSeparators = new string[] { Environment.NewLine, "\n" };
            string[] lines = value.Split(stringSeparators, StringSplitOptions.None);
            string[] response = lines;
            return response;
        }

        public static string ExtractLine(string value, string search, params string[] remove)
        {
            string response = "";
            string[] lines = SplitLines(value);
            foreach (string l in lines)
            {
                if (l.Contains(search) && !String.IsNullOrEmpty(search))
                {
                    response = Strings.RemoveWords(l, remove);
                    break;
                }
            }
            return response;
        }
    }
}

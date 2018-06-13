using System;

namespace ToolBox.Validations
{
    public static class Strings
    {
        public static bool SomeNullOrEmpty(params string[] values)
        {
            try
            {
                foreach (string value in values)
                {
                    if (String.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (NullReferenceException)
            {
                return true;
            }
        }
    }
}

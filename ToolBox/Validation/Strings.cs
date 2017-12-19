using System;
using System.Runtime.InteropServices;

namespace ToolBox.Validations
{
    public static class Strings {
        public static bool SomeNullOrEmpty(params string[] values){
            bool result = false;
            try{
                foreach (var value in values)
                {
                    result = result || String.IsNullOrEmpty(value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}

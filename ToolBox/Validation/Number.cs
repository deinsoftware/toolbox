using System;
using System.Runtime.InteropServices;

namespace ToolBox.Validations
{
    public static class Number
    {
        public static bool IsNumber(string value)
        {
            try
            {
                int number;
                bool isNumeric = int.TryParse(value, out number);
                return (isNumeric);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public static bool IsOnRange(int min, int value, int max)
        {
            try
            {
                return (min <= value && value <= max);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

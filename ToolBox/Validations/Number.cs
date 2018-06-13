using System;

namespace ToolBox.Validations
{
    public static class Number
    {
        public static bool IsNumber(string value)
        {
            int number;
            bool isNumeric = int.TryParse(value, out number);
            return (isNumeric);
        }

        public static bool IsOnRange(int min, int value, int max)
        {
            return (min <= value && value <= max);
        }
    }
}

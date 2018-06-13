using System;

namespace ToolBox.Validations
{
    public static class Bool
    {
        public static bool SomeFalse(params bool[] values)
        {
            try
            {
                foreach (bool value in values)
                {
                    if (!value)
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

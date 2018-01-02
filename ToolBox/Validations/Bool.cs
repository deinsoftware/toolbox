using System;

namespace ToolBox.Validations
{
    public static class Bool {
        public static bool SomeFalse(params bool[] values){
            try{
                foreach (bool value in values)
                {
                    if (value == false){
                        return true;
                    }
                }
                return false;
            }
            catch (NullReferenceException)
            {
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

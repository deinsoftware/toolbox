using System;

namespace ToolBox.Validations
{
    public static class Web
    {
        public static bool IsUrl(string uriName)
        {
            bool result = false;
            Uri uriResult;
            result = 
                Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}

using System.Collections.Generic;
using System.IO;

namespace ToolBox.Files
{
    public interface INotificationSystem
    {
        void ShowAction(string action, string message);
    }
}
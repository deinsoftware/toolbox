using System;
using System.Collections.Generic;
using System.IO;

namespace ToolBox.Files
{
    public static class NotificationSystem
    {
        public static INotificationSystem Default
        {
            get
            {
                return new DefaultNotificationSystem();
            }
        }
    }

    public sealed class DefaultNotificationSystem : INotificationSystem
    {
        public void ShowAction(string action, string message)
        {
            //do nothing;
        }
    }
}
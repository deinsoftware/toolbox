using System;
using System.Collections.Generic;
using System.IO;

namespace ToolBox.Notification
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
            //Do nothing
        }

        public void StandardOutput(string message)
        {
            Console.WriteLine(message);
        }

        public void StandardWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void StandardError(string message)
        {
            Console.WriteLine(message);
        }

        public void StandardLine()
        {
            Console.WriteLine("");
        }
    }
}
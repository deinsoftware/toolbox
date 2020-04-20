namespace ToolBox.Notification
{
    public interface INotificationSystem
    {
        void ShowAction(string action, string message);
        void StandardOutput(string message);
        void StandardWarning(string message);
        void StandardError(string message);
        void StandardLine();
    }
}
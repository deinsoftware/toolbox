using System;

namespace ToolBox.System.Command
{
    public interface ICommand
    {
        string PathNormalizer(string path);
        string GetUserFolder(string path);
    }
}
using System;

namespace ToolBox.System
{
    public interface ICommandSystem
    {
        string PathNormalizer(string path);
        string GetUserFolder(string path);
    }
}
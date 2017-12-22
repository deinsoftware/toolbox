using System;

namespace ToolBox.Platform
{
    public interface ICommandSystem
    {
        string PathNormalizer(string path);
        string GetHomeFolder(string path);
    }
}
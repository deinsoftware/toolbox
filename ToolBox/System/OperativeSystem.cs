using System;
using System.Runtime.InteropServices;

namespace ToolBox.System
{
    public static class OperativeSystem
    {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string Platform() {
            var os =    (IsWindows() ? "win" : null) ??
                        (IsMacOS()   ? "mac" : null) ??
                        (IsLinux()   ? "gnu" : null) ;
            return os;
        }
    }
}

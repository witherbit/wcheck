using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wshell.License.HWID
{
    internal static class AppInfo
    {
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsMacOs => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string WindowsInstallationDirectory => Path.GetPathRoot(Environment.SystemDirectory);

        public static bool Is64 => Environment.Is64BitOperatingSystem;
        public static string OsArch => Is64 ? "64" : "32";
        public static bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
    }
}

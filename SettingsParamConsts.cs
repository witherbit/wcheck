using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wcheck
{
    public static class SettingsParamConsts
    {
        public static readonly string Build = "0.9.6";
        public static readonly string BuildType = "beta";
        public static readonly string Company = "witherbit";
        public static readonly string Author = "Сизов А.И.";
        public static readonly string Support = "xttwercs@vk.com";
        public struct ParameterPath
        {
            public static readonly string p_PathToXds = "pXsdPath";
            public static readonly string p_PathToTemp = "pTempPath";
            public static readonly string p_PathToLog = "pLogPath";
            public static readonly string p_PathToShell = "pShellPath";
            public static readonly string p_PathToDeps = "pShellDepsPath";
            public static readonly string p_PathToShellSha = "pShellShaPath";

            internal static readonly string v_PathToXds = @"C:\ProgramData\Witherbit\wcheck\XSD";
            internal static readonly string v_PathToTemp = @"C:\ProgramData\Witherbit\wcheck\tmp";
            internal static readonly string v_PathToLog = @"C:\ProgramData\Witherbit\wcheck\log";
            internal static readonly string v_PathToShell = @"C:\ProgramData\Witherbit\wcheck\shell";
            public static readonly string v_PathToDeps = @"C:\ProgramData\Witherbit\wcheck\shell\dependencies";
            internal static readonly string v_PathToShellSha = @"https://raw.githubusercontent.com/witherbit/wcheck/master/wshell/Repos/sha.rep";
        }
        public struct ParameterShell
        {
            public static readonly string p_AcceptedShell = "pAcceptedShell";
            public static readonly string p_CheckShell = "pCheckShell";

            internal static readonly string v_AcceptedShell = "b4877dc5-a5b5-4b7e-b08b-1b1995e8c8d8\r\nee3de59e-00e8-40e9-bfcd-cba116b9a81d\r\n4f1fc6ba-56b0-4844-8e2a-485578e8bc1f";
            internal static readonly bool v_CheckShell = true;
        }

        public struct ParameterConnection
        {
            public static readonly string p_Type = "pConType";
            public static readonly string p_EncPath = "pEncPathR";
            public static readonly string p_UseEnc = "pConEnc";
            public static readonly string p_Port = "pConPort";

            internal static readonly int v_Type = 0;
            internal static readonly bool v_UseEnc = true;
            internal static readonly int v_Port = 11144;
        }
    }
}

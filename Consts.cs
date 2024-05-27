using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wcheck
{
    static class Consts
    {
        public struct ParameterPath
        {
            public static readonly string p_PathToXds = "pXsdPath";
            public static readonly string p_PathToTemp = "pTempPath";
            public static readonly string p_PathToLog = "pLogPath";
            public static readonly string p_PathToShell = "pShellPath";
            public static readonly string p_PathToShellSha = "pShellShaPath";

            public static readonly string v_PathToXds = @"C:\ProgramData\Witherbit\wcheck\XSD";
            public static readonly string v_PathToTemp = @"C:\ProgramData\Witherbit\wcheck\tmp";
            public static readonly string v_PathToLog = @"C:\ProgramData\Witherbit\wcheck\log";
            public static readonly string v_PathToShell = @"C:\ProgramData\Witherbit\wcheck\shell";
            public static readonly string v_PathToShellSha = @"C:\ProgramData\Witherbit\wcheck\shell\sha";
        }
        public struct ParameterShell
        {
            public static readonly string p_AcceptedShell = "pAcceptedShell";
            public static readonly string p_CheckShell = "pCheckShell";

            public static readonly string v_AcceptedShell = "b4877dc5-a5b5-4b7e-b08b-1b1995e8c8d8\r\nb4877dc5-a5b5-4b7e-b08b-1b1995e8c8d8";
            public static readonly bool v_CheckShell = true;
        }

        public struct ParameterConnection
        {
            public static readonly string p_Type = "pConType";
            public static readonly string p_UseEnc = "pConEnc";
            public static readonly string p_Port = "pConPort";

            public static readonly int v_Type = 0;
            public static readonly bool v_UseEnc = true;
            public static readonly int v_Port = 11144;
        }
    }
}

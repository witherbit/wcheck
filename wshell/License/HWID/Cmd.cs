﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wshell.License.HWID
{
    internal class Cmd
    {
        public Process CreateHiddenProcess(string appPath, string args, CmdOptions k)
        {
            var o = k ?? CmdOptions.Default;

            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    Arguments = args,
                    CreateNoWindow = o.CreateNoWindow,
                    WindowStyle = o.WindowStyle,
                    RedirectStandardOutput = o.RedirectStdOut,
                    UseShellExecute = o.UseOsShell,
                    WorkingDirectory = o.WorkingDirectory
                }
            };
        }

        public Process CreateHiddenProcess(string appPath, string args)
        {
            return CreateHiddenProcess(appPath, args, CmdOptions.Default);
        }

        public CommandLineRunResult Run(string appPath, string args, CmdOptions o, bool waitForExit)
        {
            var process = CreateHiddenProcess(appPath, args, o);

            var res = new CommandLineRunResult
            {
                AppPath = appPath,
                Args = args
            };

            try
            {
                if (waitForExit)
                {
                    process.Start();
                    res.Output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    res.ExitType = CommandLineExitTypes.Ok;
                    res.ExitCode = process.ExitCode;
                    if (process.HasExited == false) process.Kill();
                }
                else
                {
                    process.Start();
                    res.Output = process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                res.ExitCode = int.MaxValue;
                res.ExitType = CommandLineExitTypes.ExceptionBeforeRun;
                res.Msg = ex.ToString();
            }

            return res;
        }

        public CommandLineRunResult Run(string appPath, string args, bool waitForExit)
        {
            return Run(appPath, args, CmdOptions.Default, waitForExit);
        }
    }

    internal class CmdOptions
    {
        static CmdOptions()
        {
            Default = new CmdOptions();
        }

        public CmdOptions()
        {
        }

        public CmdOptions(bool useOsShell = false, bool createNoWindow = true,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden, bool redirectStdOut = true)
            : this()
        {
            UseOsShell = useOsShell;
            CreateNoWindow = createNoWindow;
            WindowStyle = windowStyle;
            RedirectStdOut = redirectStdOut;
        }

        public static CmdOptions Default { get; }
        public bool UseOsShell { get; set; }
        public bool CreateNoWindow { get; set; } = true;
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Hidden;
        public bool RedirectStdOut { get; set; } = true;
        public string WorkingDirectory { get; set; }
    }

    internal enum CommandLineExitTypes
    {
        ExceptionBeforeRun,
        Ok
    }

    internal class CommandLineRunResult
    {
        public string AppPath { get; set; }
        public string Args { get; set; }
        public CommandLineExitTypes ExitType { get; set; }
        public int ExitCode { get; set; }
        public string Msg { get; set; }
        public string Output { get; set; }
        public string Command => $"{AppPath}{Args}";
    }
}

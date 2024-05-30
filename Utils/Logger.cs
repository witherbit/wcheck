using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wcheck.Utils
{
    public static class Logger
    {
        public static event EventHandler<LogContent> LogWrite;
        public static string Path => ShellHost.Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToLog);
        public static void Log(LogContent content)
        {
            LogWrite?.Invoke(content.Sender, content);
            var log = string.Empty;
            if (content.Exception != null)
                log = $"[{DateTime.Now}] [{content.Type}] from {content.Sender} >: {content.Message}\n\tException: {content.Exception.ToString()}\n";
            else
                log = $"[{DateTime.Now}] [{content.Type}] from {content.Sender} >: {content.Message}\n";
            try
            {
                File.AppendAllText($"{Path}/{DateTime.Now.ToString("dd.MM.yy").Replace(".", "_")}.txt", log);
            }
            catch
            {

            }
        }
        public static void Clear()
        {
            try
            {
                foreach (var file in Directory.GetFiles(Path))
                {
                    try
                    {
                        if ((DateTime.Now - Convert.ToDateTime(System.IO.Path.GetFileName(file).Replace("_", ".").Replace(".txt", ""))).TotalDays > 7)
                            File.Delete(file);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
    }
    public enum LogType
    {
        INFO,
        WARN,
        CRITICAL
    }

    public struct LogContent
    {
        public string Message { get; set; }
        public LogType Type { get; set; }
        public object Sender { get; set; }
        public Exception Exception { get; set; }

        public LogContent(string message, object sender, LogType type = LogType.INFO, Exception exception = null)
        {
            Message = message;
            Type = type;
            Sender = sender;
            Exception = exception;
        }
    }
}

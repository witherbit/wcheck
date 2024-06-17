using Newtonsoft.Json;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wcheck.wcontrols;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Net.Nodes;

namespace wcheck.wshell.Utils
{
    public static class NetHandleExtensions
    {
        public static Node HandleNode(this Node node)
        {
            if(node.Tag == "system params" && node.GetAttribute("type") == "request")
            {
                return node.HandleSystemParameters();
            }
            else if (node.Tag == "redirect request")
            {
                return node.HandleRedirect();
            }
            else if (node.Tag == "run shell")
            {
                return node.HandleRunShell();
            }
            return new Node("empty", new Dictionary<string, string>
                {
                    { "code", "501" }
                });
        }

        public static Node HandleSystemParameters(this Node node)
        {
            return new Node("system params", new Dictionary<string, string>
            {
                { "type", "response" },
                { "os",  GetOS() },
                { "version",  GetVersion() },
                { "machine name",  GetMachineName() },
                { "user",  GetUser() },
            });
        }

        public static Node HandleRedirect(this Node node)
        {
            var target = node.GetAttribute("target");
            ShellHost.Log($"Handle redirect request for {target}: {JsonConvert.SerializeObject(node, Formatting.Indented)}");
            var schema = ShellHost.InvokeRequest(target, new Schema(Enums.CallbackType.RedirectNetRequest).SetProviding(node).SetAttribute("target", target));
            ShellHost.Log($"Returned redirect response data: {JsonConvert.SerializeObject(schema, Formatting.Indented)}");
            if (schema.Type == Enums.CallbackType.EmptyResponse)
                return new Node("underfined redirect response", new Dictionary<string, string>
                {
                    { "code", "404" }
                });
            var rnode = schema.GetProviding<Node>();
            if(rnode != null)
            {
                rnode.Tag = "redirect response";
                return rnode;
            }
            return new Node("underfined redirect response", new Dictionary<string, string>
                {
                    { "code", "404" }
                });
        }

        public static Node HandleRunShell(this Node node)
        {
            ShellHost.Log($"Handle run shell request: {JsonConvert.SerializeObject(node, Formatting.Indented)}");
            ShellHost.Instance.HostWindow.Invoke(() =>
            {
                ShellHost.Instance.Controller.GetShellById(node.GetAttribute("target")).Run();
            });
            return new Node("run shell accepted").SetAttribute("code", "200");
        }

        private static string GetOS()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
        private static string GetVersion()
        {
            return Environment.OSVersion.Version.ToString();
        }
        private static string GetMachineName()
        {
            return Environment.MachineName.ToString();
        }
        private static string GetUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
    }
    public partial struct NetSchema
    {
    }
}

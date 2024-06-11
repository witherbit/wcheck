using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

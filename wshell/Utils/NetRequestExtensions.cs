using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using wshell.Net;
using wshell.Net.Nodes;

namespace wcheck.wshell.Utils
{
    public static class NetRequestExtensions
    {
        internal async static Task<NetSchema.SystemParameters?> GetSystemParametersAsync(this ShellSocket shellSocket, string target)
        {
            var node = await shellSocket.RequestAsync(IPAddress.Parse(target), new Node("system params").SetAttribute("type", "request"), shellSocket.CancellationToken);
            if (node.Tag != "system params")
                return null;
            return new NetSchema.SystemParameters
            {
                Os = node.GetAttribute("os"),
                OsVersion = node.GetAttribute("version"),
                UserName = node.GetAttribute("user"),
                MachineName = node.GetAttribute("machine name"),
            };
        }
        public async static Task<NetSchema.SystemParameters?> GetSystemParametersAsync(this ShellClientProviding clientProviding, string target)
        {
            return await clientProviding._socket.GetSystemParametersAsync(target);
        }
    }

    public partial struct NetSchema
    {
        public class SystemParameters
        {
            public string Os {  get; set; }
            public string UserName { get; set; }
            public string MachineName { get; set; }
            public string OsVersion { get; set; }
        }
    }
}

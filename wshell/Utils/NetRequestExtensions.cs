using Newtonsoft.Json;
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
            ShellHost.Log($"Sending node to {target}: {JsonConvert.SerializeObject(node, Formatting.Indented)}");
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
        internal async static Task<Node> GetRedirectAsync(this ShellSocket shellSocket, string target, string shellId, Node node)
        {
            node.Tag = "redirect request";
            node.SetAttribute("target", shellId);
            ShellHost.Log($"Sending node to {target}: {JsonConvert.SerializeObject(node, Formatting.Indented)}");
            var rnode = await shellSocket.RequestAsync(IPAddress.Parse(target), node, shellSocket.CancellationToken);
            return rnode;
        }
        internal async static Task<Node> GetRunShellAsync(this ShellSocket shellSocket, string target, string shellId)
        {
            var node = new Node("run shell");
            node.SetAttribute("target", shellId);
            ShellHost.Log($"Sending node to {target}: {JsonConvert.SerializeObject(node, Formatting.Indented)}");
            var rnode = await shellSocket.RequestAsync(IPAddress.Parse(target), node, shellSocket.CancellationToken);
            return rnode;
        }
        public async static Task<NetSchema.SystemParameters?> GetSystemParametersAsync(this ShellClientProviding clientProviding, string target)
        {
            return await clientProviding._socket.GetSystemParametersAsync(target);
        }

        public async static Task<Node> GetRedirectAsync(this ShellClientProviding clientProviding, string target, string shellId, Node node)
        {
            return await clientProviding._socket.GetRedirectAsync(target, shellId, node);
        }

        public async static Task<Node> GetRunShellAsync(this ShellClientProviding clientProviding, string target, string shellId)
        {
            return await clientProviding._socket.GetRunShellAsync(target, shellId);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using wshell.Abstract;
using wshell.Enums;
using wshell.Net.Nodes;

namespace wshell.Net
{
    public sealed class ShellClientProviding
    {
        internal ShellSocket _socket;
        public CancellationToken CancellationToken => _socket.CancellationToken;
        public NetworkingConnectionType ConnectionType => _socket.InitializeParameters.ConnectionType;
        public async Task<Node> RequestAsync(string target, Node node)
        {
            return await _socket.RequestAsync(IPAddress.Parse(target), node, CancellationToken);
        }
    }
}

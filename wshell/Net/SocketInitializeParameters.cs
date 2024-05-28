using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.Enums;

namespace wshell.Net
{
    public sealed class SocketInitializeParameters
    {
        public int Port { get; set; }
        SocketAuthType AuthType { get; set; }
    }
}

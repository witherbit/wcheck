using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wshell.Objects
{
    internal struct RevokeToken
    {
        public Guid ShellId { get; internal set; }
        internal CancellationToken _cancellationToken {  get; set; }

        public bool IsRevokeRequested => _cancellationToken.IsCancellationRequested;

        public void Register(Action callback)
        {
            _cancellationToken.Register(callback);
        }

    }
}

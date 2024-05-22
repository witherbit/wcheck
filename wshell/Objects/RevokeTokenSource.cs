using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using wshell.Abstract;

namespace wshell.Objects
{
    internal class RevokeTokenSource
    {
        public RevokeToken Token { get; }
        private CancellationTokenSource _tokenSource { get; set; }
        public RevokeTokenSource(ShellBase shell)
        {
            _tokenSource = new CancellationTokenSource();
            Token = new RevokeToken
            {
                _cancellationToken = _tokenSource.Token,
                ShellId = shell.ShellInfo.Id
            };
        }
        public void Revoke()
        {
            _tokenSource.Cancel();
        }
    }
}

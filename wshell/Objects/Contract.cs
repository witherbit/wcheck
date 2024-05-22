using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.wshell.Objects;
using wshell.Abstract;

namespace wshell.Objects
{
    internal sealed class Contract
    {
        private ShellBase _shell { get; set; }

        public RevokeToken RevokeToken { get; internal set; }

        public Guid Id { get; }

        public Guid DestinationShellId => RevokeToken.ShellId;

        public Contract(RevokeToken token)
        {
            Id = Guid.NewGuid();
            RevokeToken = token;
            RevokeToken.Register(() => Revoke());
        }
        private void Revoke()
        {
            if (_shell != null && _shell.Callback != null)
            {
                _shell.Stop();
                _shell.Callback = null;
                _shell.ContractId = default;
            }
        }

        internal void Register(ShellBase shell, IHost host)
        {
            if (shell.Callback == null)
            {
                _shell = shell;
                _shell.Callback = host.Callback;
                _shell.ContractId = Id;
            }
            else
            {
                throw new Exception("Contract already exist");
            }
        }

        public void Invoke(Schema schema)
        {
            _shell.OnHostCallback(schema);
        }
    }
}

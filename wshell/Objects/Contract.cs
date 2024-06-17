using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wcheck;
using wcheck.Utils;
using wcheck.wshell.Objects;
using wshell.Abstract;

namespace wshell.Objects
{
    internal sealed class Contract
    {
        private ShellBase _shell { get; set; } //модуль назначения

        public RevokeToken RevokeToken { get; internal set; } //токен отзыва

        public Guid Id { get; } //идентификатор контракта

        public Guid DestinationShellId => RevokeToken.ShellId; // идентификатор модуля назначения

        public Contract(RevokeToken token)
        {
            Id = Guid.NewGuid();
            RevokeToken = token;
            RevokeToken.Register(() => Revoke());
        }
        private void Revoke() //отзывает контракт
        {
            if (_shell != null && _shell.Callback != null)
            {
                Logger.Log(new LogContent($"Revoke contract {Id}", this));
                _shell.Stop();
                _shell.Callback = null;
                _shell.ContractId = default;
            }
        }

        internal void Register(ShellBase shell, IHost host) //регистрирует контракт
        {
            if (shell.Callback == null)
            {
                _shell = shell;
                _shell.Callback = host.Callback;
                _shell.ContractId = Id;
                Logger.Log(new LogContent($"Register contract {Id} to {shell.ShellInfo.Id} [{shell.ShellInfo.Name}]", this));
            }
            else
            {
                Logger.Log(new LogContent($"Failed to register contract {Id} to {shell.ShellInfo.Id} [{shell.ShellInfo.Name}]", this, LogType.WARN, new Exception("Contract already exist")));
            }
        }

        public Schema Invoke(Schema schema) //отправка запроса модулю от хоста через контракт
        {
            ShellHost.Log($"Invoke contract {Id} callback request to {_shell.ShellInfo.Id} [{_shell.ShellInfo.Name}] ");
            //Logger.Log(new LogContent(, this));
            return _shell.OnHostCallback(schema);
        }
    }
}

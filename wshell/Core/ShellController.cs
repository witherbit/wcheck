using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wcheck.Pages;
using wcheck.Utils;
using wshell.Abstract;
using wshell.Objects;

namespace wshell.Core
{
    internal class ShellController
    {
        private List<ShellBase> _shells = new List<ShellBase>(); //список для хранения модулей
        private List<ShellInfo> _deniedIds = new List<ShellInfo>(); //список для хранения отклоненных модулей
        public ContractStore ContractStore { get; } //хранилище контрактов
        public ShellBase[] Shells { get => _shells.Count < 1 ? Array.Empty<ShellBase>() : _shells.ToArray(); }
        public ShellInfo[] DeniedIds { get => _deniedIds.Count < 1 ? Array.Empty<ShellInfo>() : _deniedIds.ToArray(); }

        public ShellController()
        {
            ContractStore = new ContractStore();
        }

        public void LoadAll(string directory, string allowedIds) //загружает разрешенные модули из директории
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Logger.Log(new LogContent($"Try loading {System.IO.Path.GetFileName(file)}", this));
                LoadByPath(file, allowedIds);
            }
        }

        public void LoadByPath(string path, string allowedIds) //загружает один разрешенный модуль
        {
            try
            {
                var shellAssembly = Assembly.LoadFile(path);
                Logger.Log(new LogContent($"Get assembly: {shellAssembly.GetTypes().Length}", this));
                foreach (var type in shellAssembly.GetTypes())
                {
                    Logger.Log(new LogContent($"Get type: {type.Name}", this));
                    if (type.BaseType == typeof(ShellBase))
                    {
                        var shell = shellAssembly.CreateInstance(type.FullName) as ShellBase;
                        if (shell != null && _shells.FirstOrDefault(x => x.ShellInfo.Id == shell.ShellInfo.Id) == null)
                        {
                            if (allowedIds.Contains(shell.ShellInfo.Id.ToString()))
                            {
                                Logger.Log(new LogContent($"Shell {shell.ShellInfo.Id.ToString()} allowed", this));
                                _shells.Add(shell);
                                WelcomePage.AddShell(shell);
                            }
                            else
                            {
                                Logger.Log(new LogContent($"Shell {shell.ShellInfo.Id.ToString()} forbidden", this, LogType.WARN));
                                _deniedIds.Add(shell.ShellInfo);
                            }
                        }
                            
                    }
                }
            }
            catch { }
        }

        public void RegisterAll(IHost host) //регистрирует контракты всех модулей
        {
            for (int i = 0; i < _shells.Count; i++)
                RegisterShellByIndex(i, host);
        }

        public ShellBase GetShellById(Guid shellId)
        {
            return _shells.FirstOrDefault(x => x.ShellInfo.Id == shellId);
        }

        public ShellBase GetShellById(string shellId) //возвращает модуль по его идентификатору
        {
            return _shells.FirstOrDefault(x => x.ShellInfo.Id.ToString() == shellId);
        }

        public ShellBase GetShellByIndex(int index)
        {
            return _shells[index];
        }

        public ShellBase RegisterShellById(Guid shellId, IHost host) //регистрирует контракт по идентификатору модуля
        {
            var shell = _shells.FirstOrDefault(x => x.ShellInfo.Id == shellId);
            RegisterShell(shell, host);
            return shell;
        }
        public void RegisterShell(ShellBase shell, IHost host) //регистрирует контракт
        {
            ContractStore.Register(shell, host);
        }

        public ShellBase RegisterShellByIndex(int index, IHost host)
        {
            var shell = _shells[index];
            RegisterShell(shell, host);
            return shell;
        }
    }
}

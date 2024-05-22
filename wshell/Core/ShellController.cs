using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wshell.Abstract;

namespace wshell.Core
{
    internal class ShellController
    {
        private List<ShellBase> _shells = new List<ShellBase>();
        public ContractStore ContractStore { get; }
        public ShellBase[] Shells { get => _shells.Count < 1 ? Array.Empty<ShellBase>() : _shells.ToArray(); }

        public ShellController()
        {
            ContractStore = new ContractStore();
        }

        public void LoadAll(string directory)
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories))
                LoadByPath(file);
        }

        public void LoadByPath(string path)
        {
            try
            {
                var shellAssembly = Assembly.LoadFile(path);
                foreach (var type in shellAssembly.GetTypes())
                {
                    if (type.BaseType == typeof(ShellBase))
                    {
                        var shell = shellAssembly.CreateInstance(type.FullName) as ShellBase;
                        if (shell != null && _shells.FirstOrDefault(x => x.ShellInfo.Id == shell.ShellInfo.Id) == null)
                            _shells.Add(shell);
                    }
                }
            }
            catch { }
        }

        public void RegisterAll(IHost host)
        {
            for (int i = 0; i < _shells.Count; i++)
                RegisterShellByIndex(i, host);
        }

        public ShellBase GetShellById(Guid shellId)
        {
            return _shells.FirstOrDefault(x => x.ShellInfo.Id == shellId);
        }

        public ShellBase GetShellByIndex(int index)
        {
            return _shells[index];
        }

        public ShellBase RegisterShellById(Guid shellId, IHost host)
        {
            var shell = _shells.FirstOrDefault(x => x.ShellInfo.Id == shellId);
            RegisterShell(shell, host);
            return shell;
        }
        public void RegisterShell(ShellBase shell, IHost host)
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

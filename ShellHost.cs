using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wshell.Abstract;
using wshell.Core;

namespace wcheck
{
    internal class ShellHost : IHost
    {
        public MainWindow HostWindow { get; private set; }
        public static ShellHost Instance { get; private set; }
        public ShellCallback Callback { get; private set; }
        public ShellController Controller { get; private set; }

        public TabablzControl Tab { get; private set; }

        public ContractStore ContractStore => Controller.ContractStore;

        public ShellHost(MainWindow mainWindow)
        {
            Instance = this;
            HostWindow = mainWindow;
            Callback = new ShellCallback();
            Controller = new ShellController();
        }
    }
}

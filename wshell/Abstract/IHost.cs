using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.Core;

namespace wshell.Abstract
{
    internal interface IHost
    {
        ShellCallback Callback { get; } //объект для взаимодействия между модулями
        ShellController Controller { get; } //объект для контроля модулей
    }
}

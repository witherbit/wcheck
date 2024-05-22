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
        ShellCallback Callback { get; }
        ShellController Controller { get; }
    }
}

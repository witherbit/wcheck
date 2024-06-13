using pwither.ev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.Abstract;

namespace wshell.Objects
{
    public class EventRedirect : EventScript
    {
        public ShellInfo ParentShellInfo { get; set; }
        public EventRedirect(ShellBase parentShell)
        {
            ParentShellInfo = parentShell.ShellInfo;
        }
    }
}

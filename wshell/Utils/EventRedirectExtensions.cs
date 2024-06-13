using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.Abstract;
using wshell.Objects;

namespace wshell.Utils
{
    public static class EventRedirectExtensions
    {
        public static void SetEventRedirect(this ShellBase shellBase, EventRedirect eventRedirect)
        {
            shellBase._eventsRedirect = eventRedirect;
        }
        public static EventRedirect? GetEventRedirect(this ShellBase shellBase)
        {
            return shellBase._eventsRedirect;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Objects;

namespace wshell.Core
{
    public class ShellCallback
    {
        public delegate void ShellInvoke(ShellBase shell, Schema schema);
        public delegate Schema ShellInvokeRequest(ShellBase shell, Schema schema);

        //public delegate void HostInvoke(ShellInfo info, object obj);
        //public delegate object HostInvokeRequest(ShellInfo info, object obj);

        public event ShellInvoke Callback;
        public event ShellInvokeRequest RequestCallback;

        //public event HostInvoke HostInvokeCallback;
        //public event HostInvokeRequest HostInvokeReturnedCallback;

        public void Invoke(ShellBase shell, Schema schema)
        {
            Callback?.Invoke(shell, schema);
        }

        public Schema InvokeRequest(ShellBase shell, Schema schema)
        {
            return RequestCallback?.Invoke(shell, schema);
        }

        //public void HostInvokeNative(ShellInfo info, object obj, Guid id)
        //{
        //    HostInvokeCallback?.Invoke(info, obj, id);
        //}

        //public object HostInvokeReturnedNative(ShellInfo info, object obj, Guid id)
        //{
        //    return HostInvokeReturnedCallback?.Invoke(info, obj, id);
        //}
    }
}

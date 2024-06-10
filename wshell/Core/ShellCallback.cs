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
        internal delegate void ShellInvoke(ShellBase shell, Schema schema); //делегат вызова
        internal delegate Schema ShellInvokeRequest(ShellBase shell, Schema schema); //делегат запроса

        internal event ShellInvoke Callback; //событие вызова
        internal event ShellInvokeRequest RequestCallback; //событие запроса

        public void Invoke(ShellBase shell, Schema schema) //вызов
        {
            Callback?.Invoke(shell, schema);
        }

        public Schema InvokeRequest(ShellBase shell, Schema schema) //запрос
        {
            return RequestCallback?.Invoke(shell, schema);
        }
    }
}

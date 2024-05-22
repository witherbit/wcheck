using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.wshell.Enums;

namespace wcheck.wshell.Objects
{
    public sealed class Schema
    {
        public CallbackType Type { get; set; }
        private object Providing { get; set; }

        public Schema SetProviding<T>(T providing)
        {
            this.Providing = providing;
            return this;
        }
        public T GetProviding<T>()
        {
            return (T)Providing;
        }
    }
}

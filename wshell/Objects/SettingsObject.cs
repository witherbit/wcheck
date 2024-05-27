using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;

namespace wcheck.wshell.Objects
{
    [BitSerializable]
    public class SettingsObject
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}

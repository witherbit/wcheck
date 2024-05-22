using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.License.HWID;

namespace wshell.License
{
    public static class ProductId
    {
        public static string HWID => HwId.Generate();

        public static string ShellVersion => "wshell v1.0";
    }
}

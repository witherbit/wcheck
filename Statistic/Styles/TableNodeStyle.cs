using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;

namespace wcheck.Statistic.Styles
{
    [BitSerializable]
    public class TableNodeStyle
    {
        public bool UseBorders { get; set; } = true;
        public uint BordersWidth { get; set; } = 1;
        public int WidthPercent { get; set; } = 100;
    }
}

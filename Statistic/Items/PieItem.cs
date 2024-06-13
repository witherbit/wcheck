using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;

namespace wcheck.Statistic.Items
{
    [BitSerializable]
    public class PieItem
    {
        public int[] Values { get; set; }
        public string Name { get; set; }
        public string? HexColor { get; set; }
        public PieItem(string name, int[] values, string? hexColor = null) 
        {
            Name = name;
            Values = values;
            HexColor = hexColor;
        }
    }
}

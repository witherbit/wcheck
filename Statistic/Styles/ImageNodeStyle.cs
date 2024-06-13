using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.Statistic.Enums;
using pwither.formatter;

namespace wcheck.Statistic.Styles
{
    [BitSerializable]
    public class ImageNodeStyle
    {
        public TextAligment Aligment { get; set; } = TextAligment.Center;
        public double LeftIndent { get; set; } = 0;
        public double RightIndent { get; set; } = 0;
        public double FirstLineIndent { get; set; } = 0;
        public double SpacingBetweenLines { get; set; } = 1;
        public double SpacingBefore { get; set; } = 0;
        public double SpacingAfter { get; set; } = 0;
    }
}

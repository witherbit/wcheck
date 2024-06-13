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
    public class TextNodeStyle
    {
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsStrikeThrough { get; set; } = false;
        public bool IsUnderline { get; set; } = false;
        public bool IsHyperlink { get; set; } = false;
        public double FontSize { get; set; } = 14;
        public string FontFamily { get; set; } = "Times New Roman";
        public TextAligment Aligment { get; set; } = TextAligment.Left;
        public double LeftIndent { get; set; } = 0;
        public double RightIndent { get; set; } = 0;
        public double FirstLineIndent { get; set; } = 0;
        public double SpacingBetweenLines { get; set; } = 1;
        public double SpacingBefore { get; set; } = 0;
        public double SpacingAfter { get; set; } = 0;
        public string? Foreground { get; set; }
        public string? HyperlinkUrl { get; set; }

        public double WpfFontSize { get; set; } = 18;
        public string WpfFontFamily { get; set; } = "Arial";
        public WpfThinkness WpfMargin { get; set; } = new WpfThinkness(5);
        internal string WpfOverForeground => Foreground != null ? Foreground : "#1f1f1f";

    }
    [BitSerializable]
    public struct WpfThinkness
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public WpfThinkness(double all)
        {
            Left = all;
            Right = all;
            Top = all;
            Bottom = all;
        }
        public WpfThinkness(double horizontal, double vertical)
        {
            Left = horizontal;
            Right = horizontal;
            Top = vertical;
            Bottom = vertical;
        }
        public WpfThinkness(double left, double top, double right, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocxEngine.Models;
using pwither.formatter;

namespace wcheck.Statistic
{
    [BitSerializable]
    public class TableItem
    {
        public InnerText InnerText { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public TableItem(InnerText innerText, int column, int row)
        {
            InnerText = innerText;
            Row = row;
            Column = column;
        }
    }

    [BitSerializable]
    public class InnerText
    {
        public string Text { get; set; }
        public InnerTextAligment Aligment { get; set; } = InnerTextAligment.Left;
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsStrikeThrough { get; set; } = false;
        public bool IsUnderline { get; set; } = false;
        public double FontSize { get; set; } = 14;
        public string FontFamily { get; set; } = "Times New Roman";
        public double LeftIndent { get; set; } = 0;
        public double RightIndent { get; set; } = 0;
        public double FirstLineIndent { get; set; } = 0;
        public double SpacingBetweenLines { get; set; } = 1;
        public double SpacingBefore { get; set; } = 0;
        public double SpacingAfter { get; set; } = 0;
        public InnerText(string text)
        {
            Text = text;
        }

    }

    [BitSerializable]
    public enum InnerTextAligment
    {
        Right,
        Left,
        Center,
        Both
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocxEngine.Models;
using pwither.formatter;
using wcheck.Statistic.Enums;
using wcheck.Statistic.Styles;

namespace wcheck.Statistic.Items
{
    [BitSerializable]
    public class CeilItem
    {
        public TextNodeStyle TextStyle { get; set; }
        public string Text { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string? Fill { get; set; }
        public CeilItem(string text, int column, int row, TextNodeStyle textStyle)
        {
            TextStyle = textStyle;
            Row = row;
            Column = column;
            Text = text;
        }
        public CeilItem(string text, int column, int row, string fill, TextNodeStyle textStyle)
        {
            TextStyle = textStyle;
            Row = row;
            Column = column;
            Text = text;
            Fill = fill;
        }
        public CeilItem(string text, int column, int row, string? fill = null)
        {
            TextStyle = new TextNodeStyle();
            Row = row;
            Column = column;
            Text = text;
            Fill = fill;
        }
        public CeilItem SetFill(string hexColor)
        {
            Fill = hexColor;
            return this;
        }
    }
}

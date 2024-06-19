using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocxEngine;
using pwither.formatter;
using wcheck.Statistic.Items;
using wcheck.Statistic.Styles;
using wcheck.wcontrols;

namespace wcheck.Statistic.Nodes
{
    [BitSerializable]
    public class TableStatisticNode : IStatisticNode
    {
        public TableNodeStyle Style { get; set; }
        public List<CeilItem> Ceils { get; }

        public int Rows => GetRows();
        public int Columns => GetColumns();

        public TableStatisticNode()
        {
            Ceils = new List<CeilItem>();
            Style = new TableNodeStyle();
        }
        public TableStatisticNode(TableNodeStyle style) 
        {
            Style = style;
        }
        public TableStatisticNode(IEnumerable<CeilItem> ceils, TableNodeStyle style)
        {
            Ceils = ceils.ToList();
            Style = style;
        }
        public TableStatisticNode(IEnumerable<CeilItem> ceils)
        {
            Ceils = ceils.ToList();
            Style = new TableNodeStyle();
        }
        public string GetCSV()
        {
            throw new NotImplementedException();
        }

        public void InsertDocx(Docx doc)
        {
            doc.AddTable(new DocxEngine.Models.TableElement(Ceils, Style.ConvertToElementStyle()));
        }

        public void Render(StackPanel panel)
        {
            var grid = new Grid() { Margin = new System.Windows.Thickness(5), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center};
            var columns = GetColumns();
            var rows = GetRows();
            for (int i = 0; i < columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            var ceils = Ceils.ToArray();
            foreach (var ceil in ceils)
            {
                SetCeil(ceil, grid);
            }
            panel.Children.Add(grid);
        }

        private void SetCeil(CeilItem ceilItem, Grid grid)
        {
            var border = new Border
            {
                BorderBrush = "#1f1f1f".GetBrush()
            };
            if (ceilItem.Fill != null)
                border.Background = ceilItem.Fill.GetBrush();
            var width = (double)Style.BordersWidth;
            if(Style.UseBorders)
                if (ceilItem.Column != GetColumns() - 1)
                    if (ceilItem.Row != GetRows() - 1)
                        border.BorderThickness = new System.Windows.Thickness(width, width, 0, 0);
                    else
                        border.BorderThickness = new System.Windows.Thickness(width, width, 0, width);
                else
                    if (ceilItem.Row != GetRows() - 1)
                    border.BorderThickness = new System.Windows.Thickness(width, width, width, 0);
                else
                    border.BorderThickness = new System.Windows.Thickness(width, width, width, width);

            var text = new TextBlock
            {
                Text = ceilItem.Text,
                FontSize = ceilItem.TextStyle.WpfFontSize,
                FontFamily = new System.Windows.Media.FontFamily(ceilItem.TextStyle.WpfFontFamily),
                Margin = new System.Windows.Thickness(ceilItem.TextStyle.WpfMargin.Left, ceilItem.TextStyle.WpfMargin.Top, ceilItem.TextStyle.WpfMargin.Right, ceilItem.TextStyle.WpfMargin.Bottom),
                Foreground = ceilItem.TextStyle.WpfOverForeground.GetBrush(),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                TextWrapping = TextWrapping.WrapWithOverflow,
            };
            switch (ceilItem.TextStyle.Aligment)
            {
                case Enums.TextAligment.Right:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    break;
                case Enums.TextAligment.Left:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    break;
                case Enums.TextAligment.Center:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                case Enums.TextAligment.Both:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    break;
            }
            if (ceilItem.TextStyle.IsBold)
                text.FontWeight = FontWeights.Bold;
            if (ceilItem.TextStyle.IsItalic)
                text.FontStyle = FontStyles.Italic;
            if (ceilItem.TextStyle.IsUnderline)
                text.TextDecorations.Add(TextDecorations.Underline);
            if (ceilItem.TextStyle.IsStrikeThrough)
                text.TextDecorations.Add(TextDecorations.Strikethrough);

            if (ceilItem.TextStyle.IsHyperlink && ceilItem.TextStyle.HyperlinkUrl != null)
            {
                var defaultForeground = text.Foreground;
                text.MouseEnter += (sender, e) =>
                {
                    text.Foreground = "#fca577".GetBrush();
                };
                text.MouseLeave += (sender, e) =>
                {
                    text.Foreground = defaultForeground;
                };
                text.MouseLeftButtonUp += (sender, e) =>
                {
                    ProcessStartInfo psInfo = new ProcessStartInfo
                    {
                        FileName = ceilItem.TextStyle.HyperlinkUrl,
                        UseShellExecute = true
                    };
                    Process.Start(psInfo);
                };
                text.ToolTip = ceilItem.TextStyle.HyperlinkUrl;
                text.Cursor = System.Windows.Input.Cursors.Hand;
            }

            border.Child = text;
            Grid.SetRow(border, ceilItem.Row);
            Grid.SetColumn(border, ceilItem.Column);
            grid.Children.Add(border);
        }

        public void AddCeil(int column, int row, string text, TextNodeStyle? style = null, string? fill = null)
        {
            if (GetCeil(column, row) != null)
                return;
            var competeStyle = style != null ? style : new TextNodeStyle();
            Ceils.Add(new CeilItem(text, column, row, fill, competeStyle));
        }

        private int GetColumns()
        {
            int columns = 0;
            foreach (CeilItem item in Ceils)
            {
                if (item.Column + 1 > columns)
                    columns = item.Column + 1;
            }
            return columns;
        }
        private int GetRows()
        {
            int rows = 0;
            foreach (CeilItem item in Ceils)
            {
                if (item.Row + 1 > rows)
                    rows = item.Row + 1;
            }
            return rows;
        }
        public CeilItem? GetCeil(int column, int row)
        {
            return Ceils.FirstOrDefault(x => x.Row == row && x.Column == column);
        }
    }
}

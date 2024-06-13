using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.Statistic;
using wcheck.Statistic.Items;

namespace DocxEngine.Models
{
    public class TableElement : IDocxElement
    {
        public IEnumerable<CeilItem> TableItems { get; private set; }

        public TableElementStyle Style { get; set; }
        public TableElement(IEnumerable<CeilItem> items) 
        {
            TableItems = items;
            Style = new TableElementStyle();
        }
        public TableElement(IEnumerable<CeilItem> items, TableElementStyle style)
        {
            TableItems = items;
            Style = style;
        }
        public void InsertElement(Body body, WordprocessingDocument document)
        {
            TableProperties tableProperties = new TableProperties(
        new TableWidth { Width = Style.WidthPercent + "%", Type =  TableWidthUnitValues.Pct },
        new TableStyle { Val = "TableGrid" });

            if (Style.UseBorders)
            {
                TableBorders borders = new TableBorders(
    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = Style.BordersWidth },
    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = Style.BordersWidth },
    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = Style.BordersWidth },
    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = Style.BordersWidth },
    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = Style.BordersWidth },
    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = Style.BordersWidth });

                tableProperties.Append(borders);
            }

            

            Table table = new Table(tableProperties);

            var columns = GetColumns();
            var rows = GetRows();

            for (int r = 0; r < rows; r++)
            {
                TableRow row = new TableRow();
                for (int c = 0; c < columns; c++)
                {
                    var item = GetTableItem(r, c);
                    var paragraph = new ParagraphElement(item.Text, item.TextStyle.ConvertToElementStyle());

                    TableCell cell = new TableCell(paragraph.GetParagraph());
                    if(item.Fill != null)
                    {
                        cell.Append(new TableCellProperties(new Shading() { Val = ShadingPatternValues.Clear, Fill = item.Fill.Replace("#", "") }));
                    }
                    row.Append(cell);
                }
                table.Append(row);
            }
            body.Append(table);
        }

        private int GetColumns()
        {
            int columns = 0;
            foreach (CeilItem item in TableItems)
            {
                if (item.Column + 1 > columns)
                    columns = item.Column + 1;
            }
            return columns;
        }
        private int GetRows()
        {
            int rows = 0;
            foreach (CeilItem item in TableItems)
            {
                if (item.Row + 1 > rows)
                    rows = item.Row + 1;
            }
            return rows;
        }
        private CeilItem? GetTableItem(int row, int column)
        {
            return TableItems.FirstOrDefault(x => x.Row == row && x.Column == column);
        }

        public TableElement SetBorders(bool useTableBorders)
        {
            Style.UseBorders = useTableBorders;
            return this;
        }
        public TableElement SetBordersWidth(uint width)
        {
            Style.BordersWidth = width;
            return this;
        }
        public TableElement SetBordersWidth(int percent)
        {
            if(percent > 100)
                percent = 100;
            else if(percent < 0)
                percent = 0;
            Style.WidthPercent = percent;
            return this;
        }
    }

    public class TableElementStyle
    {
        public bool UseBorders { get; set; } = true;
        public uint BordersWidth { get; set; } = 1;
        public int WidthPercent { get; set; } = 100;
    }
    
}

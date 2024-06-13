using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocxEngine;
using pwither.formatter;
using wcheck.Statistic.Items;
using wcheck.Statistic.Styles;

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
            throw new NotImplementedException();
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

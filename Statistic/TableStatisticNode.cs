using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocxEngine;
using DocxEngine.Models;

namespace wcheck.Statistic
{
    [BitSerializable]
    public class TableStatisticNode : IStatisticNode
    {
        public string Name { get; }
        public string Header { get; }
        public string ShellName { get; set; } = "wcheck";

        public IEnumerable<TableItem> TableItems { get; }

        public TableStatisticNode(string name, string header, IEnumerable<TableItem> tableItems)
        {
            Name = name;
            Header = header;
            TableItems = tableItems;
        }

        public string GetCSV()
        {
            throw new NotImplementedException();
        }

        public void InsertDocx(Docx doc)
        {
            doc.AddParagraph(new ParagraphElement()
                .SetRun(Header).SetAligment(JustificationValues.Center));
            doc.AddParagraph(new ParagraphElement());
            var desc = ShellName == "wcheck" ? "ПО" : "Модуль";
            doc.AddParagraph(new ParagraphElement()
                .SetRun($"{desc}: {ShellName}").SetAligment(JustificationValues.Right).SetItalic());
            doc.AddParagraph(new ParagraphElement());
            doc.AddTable(new TableElement(TableItems));
            doc.AddImage(new ImageElement(@"C:\Users\xttwercs\Desktop\Диплом\Diagrams\TasksCallback.png"));
            //var columns = GetColumns();
            //var rows = GetRows();

            //Table table = doc.AddTable().AddColumns(columns, 1);

            //for (int r = 0; r < rows; r++)
            //    for (int c = 0; c < columns; c++)
            //    {
            //        var item = GetTableItem(r, c);
            //        table.AddRow(row =>
            //        {
            //            row.GetCell(c).SetParagraph(item.Content, run =>
            //            {
            //                if (item.IsBold)
            //                    run.SetBold();
            //            }).(2);
            //        });
            //    }
        }

        public object Render()
        {
            throw new NotImplementedException();
        }

        private int GetColumns()
        {
            int columns = 0;
            foreach (TableItem item in TableItems)
            {
                if(item.Column + 1 > columns)
                    columns = item.Column + 1;
            }
            return columns;
        }
        private int GetRows()
        {
            int rows = 0;
            foreach (TableItem item in TableItems)
            {
                if (item.Row + 1 > rows)
                    rows = item.Row + 1;
            }
            return rows;
        }
        private TableItem? GetTableItem(int row, int column)
        {
            return TableItems.FirstOrDefault(x => x.Row == row && x.Column == column);
        }
    }
}

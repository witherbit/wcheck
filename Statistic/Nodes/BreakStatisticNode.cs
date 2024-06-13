using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DocxEngine;
using pwither.formatter;

namespace wcheck.Statistic.Nodes
{
    [BitSerializable]
    public class BreakStatisticNode : IStatisticNode
    {
        public string GetCSV()
        {
            return null;
        }

        public void InsertDocx(Docx doc)
        {
            doc.AddBreak(new DocxEngine.Models.BreakElement());
        }

        public void Render(StackPanel panel)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using pwither.formatter;
using DocumentFormat.OpenXml.Wordprocessing;
using DocxEngine;

namespace wcheck.Statistic
{
    [BitSerializable]
    public class StatisticEngine
    {
        public string Title { get; set; }
        public List<IStatisticNode> Nodes = new List<IStatisticNode> ();

        public void CreateDocx()
        {
            string fileName = @"C:\Users\xttwercs\Documents\test.docx";
            var doc = Docx.Create(fileName, true);
            foreach (var node in Nodes)
                node.InsertDocx(doc);
            doc.Save();
        }
    }
}

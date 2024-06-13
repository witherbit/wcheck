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
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Templates;
using System.Windows.Controls;
using wcheck.Statistic.Styles;

namespace wcheck.Statistic
{
    [BitSerializable]
    public class StatisticEngine
    {
        public List<IStatisticNode> Nodes { get; }
        public DateTime DateTime { get; }
        public DateTime UtcDateTime { get; }
        public string Header { get; }

        public StatisticEngine(string? header = null, TextNodeStyle style = null)
        {
            DateTime= DateTime.Now;
            UtcDateTime = DateTime.Now;
            Nodes = new List<IStatisticNode>();
            Header = header;
            if(header != null)
            {
                var completeStyle = style != null ? style : new TextNodeStyle();
                Nodes.Add(new TextStatisticNode(Header, completeStyle));
            }
        }

        public void ExportDocx(string fileName)
        {
            var doc = Docx.Create(fileName, true);
            foreach (var node in Nodes)
                node.InsertDocx(doc);
            doc.Save();
        }

        public void Render(StackPanel stackPanel)
        {
            foreach(var node in Nodes)
                node.Render(stackPanel);
        }

        public void InsertTemplate(IStatisticTemplate statisticTemplate)
        {
            if (statisticTemplate.Header != null)
                Nodes.Add(new TextStatisticNode(statisticTemplate.Header, statisticTemplate.HeaderStyle));
            foreach (var node in statisticTemplate.Nodes)
            {
                Nodes.Add(node);
            }
            if (statisticTemplate.UseBreakAfterTemplate)
                Nodes.Add(new BreakStatisticNode());
        }
    }
}

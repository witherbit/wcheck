using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocxEngine;
using DocxEngine.Models;
using pwither.formatter;

namespace wcheck.Statistic
{
    [BitSerializable]
    public class TaskStatisticNode : IStatisticNode
    {
        public string Name { get; }
        public string Header { get; }

        public TaskStatisticNode(string name, string header)
        {
            Name = name;
            Header = header;
        }

        public string GetCSV()
        {
            throw new NotImplementedException();
        }

        public void InsertDocx(Docx doc)
        {
            doc.AddParagraph(new ParagraphElement()
                .SetRun(Header).SetAligment(JustificationValues.Center).SetBold());
            doc.AddParagraph(new ParagraphElement());
            doc.AddParagraph(new ParagraphElement()
                .SetRun($"Выполненная задача: {Name}").SetAligment(JustificationValues.Right));
            doc.AddParagraph(new ParagraphElement()
                .SetRun($"Локальное время: {DateTime.Now.ToString()}").SetAligment(JustificationValues.Right).SetItalic().SetFontSize(10));
            doc.AddParagraph(new ParagraphElement()
                .SetRun($"UTC: {DateTime.UtcNow.ToString()}").SetAligment(JustificationValues.Right).SetItalic().SetFontSize(10));
            doc.AddParagraph(new ParagraphElement());
        }

        public object Render()
        {
            throw new NotImplementedException();
        }
    }
}

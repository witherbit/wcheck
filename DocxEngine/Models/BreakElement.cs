using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocxEngine.Models
{
    public class BreakElement : IDocxElement
    {
        public BreakValues BreakValues { get; private set; } = BreakValues.Page;
        public BreakElement()
        {

        }
        public BreakElement(BreakValues values)
        {
            BreakValues = values;
        }
        public void InsertElement(Body body, WordprocessingDocument document)
        {
            Break pageBreak = new Break() { Type = BreakValues };
            Paragraph paragraph = new Paragraph();
            paragraph.Append(pageBreak);
            body.Append(paragraph);
        }
        public BreakElement SetBreakValues(BreakValues breakValues)
        {
            this.BreakValues = breakValues;
            return this;
        }
    }
}

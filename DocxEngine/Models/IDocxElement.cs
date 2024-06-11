using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocxEngine.Models
{
    public interface IDocxElement
    {
        void InsertElement(Body body, WordprocessingDocument document);
    }
}

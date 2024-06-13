using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using pwither.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using DocxEngine.Models;

namespace DocxEngine
{
    public class Docx : IDisposable
    {
        private bool _isFinalized = false;
        internal event EventHandler _finalize;
        internal string _fileName {  get; set; }
        public bool AutoTransfersWords { get; set; }
        internal List<IDocxElement> Elements { get; private set; }
        public Docx()
        {
            Elements = new List<IDocxElement>();
        }
        public static Docx Create(string fileName, bool autoTransfersWords = false)
        {
            return new Docx()
            {
                _fileName = fileName,
                AutoTransfersWords = autoTransfersWords
            };
        }
        public void Save()
        {
            if (!_isFinalized)
            {
                _isFinalized = true;
                using (WordprocessingDocument document = WordprocessingDocument.Create(_fileName, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = document.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new AutoHyphenation() { Val = OnOffValue.FromBoolean(AutoTransfersWords) });
                    Body body = new Body();
                    foreach (var element in Elements)
                    {
                        element.InsertElement(body, document);
                    }
                    mainPart.Document.Append(body);
                }
                _finalize?.Invoke(this, null);
            }
        }
        public Docx AddParagraph(ParagraphElement element)
        {
            Elements.Add(element);
            return this;
        }
        public Docx AddTable(TableElement element)
        {
            Elements.Add(element);
            return this;
        }
        public Docx AddImage(ImageElement element)
        {
            Elements.Add(element);
            return this;
        }
        public Docx AddBreak(BreakElement element)
        {
            Elements.Add(element);
            return this;
        }

        public void Dispose()
        {
            if(!_isFinalized)
                Save();
        }
    }
}

using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using System.Security.Policy;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System.Windows;

namespace DocxEngine.Models
{
    public class ParagraphElement : IDocxElement
    {
        public ParagraphElementStyle Style { get; set; }
        public string Text {  get; set; }
        public ParagraphElement()
        {
            Style = new ParagraphElementStyle();
        }
        public ParagraphElement(string text)
        {
            Text = text;
            Style = new ParagraphElementStyle();
        }
        public ParagraphElement(string text, ParagraphElementStyle style)
        {
            Text = text;
            Style = style;
        }
        public void InsertElement(Body body, WordprocessingDocument document)
        {
            if(Style.IsHyperlink && Style.HyperlinkUrl != null)
            {
                var cText = Text;
                foreach (var txt in Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Text = txt;
                    body.Append(GetParagraphWithHyperlink(document));
                }
                Text = cText;
            }
            else
                body.Append(GetParagraph());
        }

        public ParagraphElement SetRun(string text)
        {
            Text = text;
            return this;
        }
        public ParagraphElement SetBold()
        {
            Style.IsBold = true;
            return this;
        }
        public ParagraphElement SetItalic()
        {
            Style.IsItalic = true;
            return this;
        }
        public ParagraphElement SetStrikeThrough()
        {
            Style.IsStrikeThrough = true;
            return this;
        }
        public ParagraphElement SetUnderline()
        {
            Style.IsUnderline = true;
            return this;
        }
        public ParagraphElement SetFontSize(double size)
        {
            Style.FontSize = size;
            return this;
        }
        public ParagraphElement SetFontFamily(string fontFamily)
        {
            Style.FontFamily = fontFamily;
            return this;
        }
        public ParagraphElement SetAligment(JustificationValues aligment)
        {
            Style.Alignment = aligment;
            return this;
        }
        public ParagraphElement SetLeftIndent(double value)
        {
            Style.LeftIndent = value;
            return this;
        }
        public ParagraphElement SetRightIndent(double value)
        {
            Style.RightIndent = value;
            return this;
        }
        public ParagraphElement SetFirstLineIndent(double value)
        {
            Style.FirstLineIndent = value;
            return this;
        }
        public ParagraphElement SetSpacingBetweenLines(double value)
        {
            Style.SpacingBetweenLines = value;
            return this;
        }
        public ParagraphElement SetSpacingBefore(double value)
        {
            Style.SpacingBefore = value;
            return this;
        }
        public ParagraphElement SetSpacingAfter(double value)
        {
            Style.SpacingAfter = value;
            return this;
        }

        internal Paragraph GetParagraph()
        {
            Paragraph paragraph = new Paragraph(new Run(new Text(Text)));
            
            paragraph.ParagraphProperties = new ParagraphProperties(
            new Justification() { Val = Style.Alignment },
            new SpacingBetweenLines() { After = Convert.ToInt32(Style.SpacingAfter * 566.929).ToString(), Before = Convert.ToInt32(Style.SpacingBefore * 566.929).ToString(), Line = Convert.ToInt32(Style.SpacingBetweenLines * 240).ToString(), LineRule = LineSpacingRuleValues.Auto },
            new Indentation()
            {
                Left = Convert.ToInt32(Style.LeftIndent * 566.929).ToString(),
                Right = Convert.ToInt32(Style.RightIndent * 566.929).ToString(),
                FirstLine = Convert.ToInt32(Style.FirstLineIndent * 566.929).ToString()
            }
        );

            RunProperties runProperties = new RunProperties(
        new RunFonts() { Ascii = Style.FontFamily, HighAnsi = Style.FontFamily },
        new FontSize() { Val = (Style.FontSize * 2).ToString() });

            if (Style.IsBold)
            {
                runProperties.Append(new Bold());
            }

            if (Style.IsItalic)
            {
                runProperties.Append(new Italic());
            }

            if (Style.IsStrikeThrough)
            {
                runProperties.Append(new Strike());
            }

            if (Style.IsUnderline)
            {
                runProperties.Append(new Underline() { Val = UnderlineValues.Single });
            }

            if (Style.Color != null)
            {
                Color color = new Color() { Val = Style.Color };
                runProperties.Append(color);
            }
                

            paragraph.GetFirstChild<Run>().PrependChild(runProperties);
            return paragraph;
        }
        static int global = 0;
        internal Paragraph GetParagraphWithHyperlink(WordprocessingDocument document)
        {
            Paragraph paragraph = new Paragraph(new Run(new Text(Text)));
            if (Style.IsHyperlink && Style.HyperlinkUrl != null)
            {
                string hyperid = "hid_" + global++;
                MainDocumentPart mainPart = document.MainDocumentPart;
                mainPart.AddHyperlinkRelationship(new System.Uri(Style.HyperlinkUrl), true, hyperid);
                Hyperlink hyperlink = new Hyperlink(new Run(new Text(Text))) { Id = hyperid };
                paragraph = new Paragraph(new Run(hyperlink));
            }

            paragraph.ParagraphProperties = new ParagraphProperties(
            new Justification() { Val = Style.Alignment },
            new SpacingBetweenLines() { After = Convert.ToInt32(Style.SpacingAfter * 566.929).ToString(), Before = Convert.ToInt32(Style.SpacingBefore * 566.929).ToString(), Line = Convert.ToInt32(Style.SpacingBetweenLines * 240).ToString(), LineRule = LineSpacingRuleValues.Auto },
            new Indentation()
            {
                Left = Convert.ToInt32(Style.LeftIndent * 566.929).ToString(),
                Right = Convert.ToInt32(Style.RightIndent * 566.929).ToString(),
                FirstLine = Convert.ToInt32(Style.FirstLineIndent * 566.929).ToString()
            }
        );

            RunProperties runProperties = new RunProperties(
        new RunFonts() { Ascii = Style.FontFamily, HighAnsi = Style.FontFamily },
        new FontSize() { Val = (Style.FontSize * 2).ToString() });

            if (Style.IsBold)
            {
                runProperties.Append(new Bold());
            }

            if (Style.IsItalic)
            {
                runProperties.Append(new Italic());
            }

            if (Style.IsStrikeThrough)
            {
                runProperties.Append(new Strike());
            }

            if (Style.IsUnderline)
            {
                runProperties.Append(new Underline() { Val = UnderlineValues.Single });
            }

            if (Style.Color != null)
                runProperties.Append(new Color() { Val = Style.Color });
            else
                runProperties.Append(new Color() { Val = "#0006ff" });



            paragraph.GetFirstChild<Run>().PrependChild(runProperties);
            return paragraph;
        }
    }
    public class ParagraphElementStyle
    {
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsStrikeThrough { get; set; } = false;
        public bool IsUnderline { get; set; } = false;
        public bool IsHyperlink { get; set; } = false;
        public double FontSize { get; set; } = 14;
        public string FontFamily { get; set; } = "Times New Roman";
        public JustificationValues Alignment { get; set; } = JustificationValues.Left;
        public double LeftIndent { get; set; } = 0;
        public double RightIndent { get; set; } = 0;
        public double FirstLineIndent { get; set; } = 0;
        public double SpacingBetweenLines { get; set; } = 1;
        public double SpacingBefore { get; set; } = 0;
        public double SpacingAfter { get; set; } = 0;
        public string? Color { get; set; }
        public string? HyperlinkUrl { get; set; }
    }
}

   

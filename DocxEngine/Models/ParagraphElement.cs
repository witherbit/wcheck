using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;
using DocumentFormat.OpenXml.Packaging;

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

            paragraph.GetFirstChild<Run>().PrependChild(runProperties);
            return paragraph;
        }
    }
    [BitSerializable]
    public class ParagraphElementStyle
    {
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsStrikeThrough { get; set; } = false;
        public bool IsUnderline { get; set; } = false;
        public double FontSize { get; set; } = 14;
        public string FontFamily { get; set; } = "Times New Roman";
        public JustificationValues Alignment { get; set; } = JustificationValues.Left;
        public double LeftIndent { get; set; } = 0;
        public double RightIndent { get; set; } = 0;
        public double FirstLineIndent { get; set; } = 0;
        public double SpacingBetweenLines { get; set; } = 1;
        public double SpacingBefore { get; set; } = 0;
        public double SpacingAfter { get; set; } = 0;
    }
}

   

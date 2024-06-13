using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DocumentFormat.OpenXml.Wordprocessing;
using DocxEngine;
using pwither.formatter;
using wcheck.Statistic.Styles;
using wcheck.wcontrols;

namespace wcheck.Statistic.Nodes
{
    [BitSerializable]
    public class TextStatisticNode : IStatisticNode
    {
        public TextNodeStyle Style { get; }
        public string Text {  get; set; }

        public TextStatisticNode()
        {
            Text = string.Empty;
            Style = new TextNodeStyle();
        }
        public TextStatisticNode(TextNodeStyle style)
        {
            Text = string.Empty;
            Style = style;
        }
        public TextStatisticNode(string text) 
        {
            Text = text;
            Style = new TextNodeStyle();
        }
        public TextStatisticNode(string text, TextNodeStyle style)
        {
            Text = text;
            Style = style;
        }
        public string GetCSV()
        {
            throw new NotImplementedException();
        }

        public void InsertDocx(Docx doc)
        {
            doc.AddParagraph(new DocxEngine.Models.ParagraphElement(Text, Style.ConvertToElementStyle()));
        }

        public void Render(StackPanel panel)
        {
            var text = new TextBlock
            {
                Text = Text,
                FontSize = Style.WpfFontSize,
                FontFamily = new System.Windows.Media.FontFamily(Style.WpfFontFamily),
                Margin = new System.Windows.Thickness(Style.WpfMargin.Left, Style.WpfMargin.Top, Style.WpfMargin.Right, Style.WpfMargin.Bottom),
                Foreground = Style.WpfOverForeground.GetBrush(),
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            switch (Style.Aligment)
            {
                case Enums.TextAligment.Right:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    break;
                case Enums.TextAligment.Left:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    break;
                case Enums.TextAligment.Center:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                case Enums.TextAligment.Both:
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    break;
            }
            if (Style.IsBold)
                text.FontWeight = FontWeights.Bold;
            if (Style.IsItalic)
                text.FontStyle = FontStyles.Italic;
            if (Style.IsUnderline)
                text.TextDecorations.Add(TextDecorations.Underline);
            if (Style.IsStrikeThrough)
                text.TextDecorations.Add(TextDecorations.Strikethrough);

            if (Style.IsHyperlink && Style.HyperlinkUrl != null)
            {
                var defaultForeground = text.Foreground;
                text.MouseEnter += (sender, e) =>
                {
                    text.Foreground = "#fca577".GetBrush();
                };
                text.MouseLeave += (sender, e) =>
                {
                    text.Foreground = defaultForeground;
                };
                text.MouseLeftButtonUp += (sender, e) =>
                {
                    ProcessStartInfo psInfo = new ProcessStartInfo
                    {
                        FileName = Style.HyperlinkUrl,
                        UseShellExecute = true
                    };
                    Process.Start(psInfo);
                };
                text.ToolTip = Style.HyperlinkUrl;
                text.Cursor = Cursors.Hand;
            }

            panel.Children.Add(text);
        }
    }
}

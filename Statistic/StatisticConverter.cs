using DocumentFormat.OpenXml.Wordprocessing;
using DocxEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using wcheck.Statistic.Enums;
using wcheck.Statistic.Styles;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WPF;

namespace wcheck.Statistic
{
    internal static class StatisticConverter
    {
        public static ParagraphElementStyle ConvertToElementStyle(this TextNodeStyle nodeStyle)
        {
            var aligment = JustificationValues.Left;
            switch (nodeStyle.Aligment)
            {
                case TextAligment.Right:
                    aligment = JustificationValues.Right;
                    break;
                case TextAligment.Center:
                    aligment = JustificationValues.Center;
                    break;
                case TextAligment.Both:
                    aligment = JustificationValues.Both;
                    break;
            }
            return new ParagraphElementStyle
            {
                IsBold = nodeStyle.IsBold,
                IsItalic = nodeStyle.IsItalic,
                IsStrikeThrough = nodeStyle.IsStrikeThrough,
                IsUnderline = nodeStyle.IsUnderline,
                IsHyperlink = nodeStyle.IsHyperlink,
                FontSize = nodeStyle.FontSize,
                FontFamily = nodeStyle.FontFamily,
                Alignment = aligment,
                LeftIndent = nodeStyle.LeftIndent,
                RightIndent = nodeStyle.RightIndent,
                FirstLineIndent = nodeStyle.FirstLineIndent,
                SpacingBetweenLines = nodeStyle.SpacingBetweenLines,
                SpacingBefore = nodeStyle.SpacingBefore,
                SpacingAfter = nodeStyle.SpacingAfter,
                Color = nodeStyle.Foreground,
                HyperlinkUrl = nodeStyle.HyperlinkUrl,
            };
        }
        public static TableElementStyle ConvertToElementStyle(this TableNodeStyle nodeStyle)
        {
            return new TableElementStyle
            {
                BordersWidth = nodeStyle.BordersWidth,
                UseBorders = nodeStyle.UseBorders,
                WidthPercent = nodeStyle.WidthPercent,
            };
        }
        public static ParagraphElementStyle ConvertToElementStyle(this ImageNodeStyle nodeStyle)
        {
            var aligment = JustificationValues.Left;
            switch (nodeStyle.Aligment)
            {
                case TextAligment.Right:
                    aligment = JustificationValues.Right;
                    break;
                case TextAligment.Center:
                    aligment = JustificationValues.Center;
                    break;
                case TextAligment.Both:
                    aligment = JustificationValues.Both;
                    break;
            }
            return new ParagraphElementStyle
            {
                Alignment = aligment,
                LeftIndent = nodeStyle.LeftIndent,
                RightIndent = nodeStyle.RightIndent,
                FirstLineIndent = nodeStyle.FirstLineIndent,
                SpacingBetweenLines = nodeStyle.SpacingBetweenLines,
                SpacingBefore = nodeStyle.SpacingBefore,
                SpacingAfter = nodeStyle.SpacingAfter,
            };
        }

        public static byte[] ConvertElementToJpg(this UIElement source, double scale, int quality)
        {
            double actualHeight = source.RenderSize.Height;
            double actualWidth = source.RenderSize.Width;

            double renderHeight = actualHeight * scale;
            double renderWidth = actualWidth * scale;

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
            VisualBrush sourceBrush = new VisualBrush(source);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                drawingContext.PushTransform(new ScaleTransform(scale, scale));
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }
            renderTarget.Render(drawingVisual);

            JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
            jpgEncoder.QualityLevel = quality;
            jpgEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            Byte[] _imageArray;

            using (MemoryStream outputStream = new MemoryStream())
            {
                jpgEncoder.Save(outputStream);
                _imageArray = outputStream.ToArray();
            }

            return _imageArray;
        }
    }
}

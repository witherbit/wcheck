using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace DocxEngine.Models
{
    public class ImageElement : IDocxElement
    {
        public string _imagePath {  get; set; }
        public ParagraphElementStyle Style { get; set; }

        public ImageElement(string imagePath)
        {
            _imagePath = imagePath;
            Style = new ParagraphElementStyle();
            SetAligment(JustificationValues.Center);
        }
        public ImageElement(string imagePath, ParagraphElementStyle style)
        {
            _imagePath = imagePath;
            Style = style;
        }
        public void InsertElement(Body body, WordprocessingDocument document)
        {
            MainDocumentPart mainPart = document.MainDocumentPart;
            string extension = System.IO.Path.GetExtension(_imagePath);
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
            switch (extension.ToLower())
            {
                case ".png":
                    imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                    break;
                case ".gif":
                    imagePart = mainPart.AddImagePart(ImagePartType.Gif);
                    break;
            }
            

            using (FileStream stream = new FileStream(_imagePath, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }

            var element = AddImageToBody(document, mainPart.GetIdOfPart(imagePart));

            body.Append(GetParagraph(element));
        }
        private Drawing AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            ImageSize imageSize = GetImageSize(_imagePath);
            long imageWidthEMU = ConvertSize(imageSize.Width);
            long imageHeightEMU = ConvertSize(imageSize.Height);
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = imageWidthEMU, Cy = imageHeightEMU },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = 1U,
                             Name = $"Picture {Guid.NewGuid().ToString()}"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = $"{Guid.NewGuid().ToString()}.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        $"{Guid.NewGuid().ToString().ToUpper()}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.HighQualityPrint
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = imageWidthEMU, Cy = imageHeightEMU }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                     });

            if (wordDoc.MainDocumentPart is null)
            {
                throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
            }
            return element;
        }

        private ImageSize GetImageSize(string imagePath)
        {
            using Image image = Image.FromFile(imagePath);
            return new ImageSize { Width = image.Width, Height = image.Height };
        }
        private long ConvertSize(long value)
        {
            const double inchInCm = 2.54;
            double inchPerPixel = 1.0 / 100;
            double cmPerPixel = inchPerPixel * inchInCm;
            double cm = value * cmPerPixel;
            return (long)cm * 360000;
        }
        private class ImageSize
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
        internal Paragraph GetParagraph(Drawing element)
        {
            Paragraph paragraph = new Paragraph(new Run(element));
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
            return paragraph;
        }

        public ImageElement SetAligment(JustificationValues aligment)
        {
            Style.Alignment = aligment;
            return this;
        }
        public ImageElement SetLeftIndent(double value)
        {
            Style.LeftIndent = value;
            return this;
        }
        public ImageElement SetRightIndent(double value)
        {
            Style.RightIndent = value;
            return this;
        }
        public ImageElement SetFirstLineIndent(double value)
        {
            Style.FirstLineIndent = value;
            return this;
        }
        public ImageElement SetSpacingBetweenLines(double value)
        {
            Style.SpacingBetweenLines = value;
            return this;
        }
        public ImageElement SetSpacingBefore(double value)
        {
            Style.SpacingBefore = value;
            return this;
        }
        public ImageElement SetSpacingAfter(double value)
        {
            Style.SpacingAfter = value;
            return this;
        }
    }
}

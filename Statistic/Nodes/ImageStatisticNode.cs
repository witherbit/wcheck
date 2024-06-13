using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DocxEngine;
using pwither.formatter;
using wcheck.Statistic.Items;
using wcheck.Statistic.Styles;

namespace wcheck.Statistic.Nodes
{
    [BitSerializable]
    public class ImageStatisticNode : IStatisticNode
    {
        public ImageNodeStyle Style { get; set; }
        public ImageItem Image { get; set; }

        public ImageStatisticNode(ImageItem image)
        {
            Image = image;
            Style = new ImageNodeStyle();
        }
        public ImageStatisticNode(ImageItem image, ImageNodeStyle style)
        {
            Image = image;
            Style = style;
        }

        public string GetCSV()
        {
            return string.Empty;
        }

        public void InsertDocx(Docx doc)
        {
            var fileName = Image.CreateTempFile();
            doc._finalize += (sender, e) => Image.RemoveTempFile();
            doc.AddImage(new DocxEngine.Models.ImageElement(fileName, Style.ConvertToElementStyle()));
        }

        public void Render(StackPanel panel)
        {
        }
    }
}

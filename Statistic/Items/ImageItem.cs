using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using pwither.formatter;
using System.Windows;

namespace wcheck.Statistic.Items
{
    [BitSerializable]
    public class ImageItem
    {
        public byte[] Raw { get; set; }
        public string PathToDirectory { get; set; }
        public string? FileName { get; private set; }
        public ImageItem(UIElement source, string pathToDirectory)
        {
            Raw = source.ConvertElementToJpg(1, 100);
            PathToDirectory = pathToDirectory;
        }
        public ImageItem(byte[] raw, string pathToDirectory)
        {
            Raw = raw;
            PathToDirectory = pathToDirectory;
        }
        public string CreateTempFile()
        {
            Directory.CreateDirectory(PathToDirectory);
            var fileName = $@"{PathToDirectory}\{Guid.NewGuid().ToString().Replace("-", "")}.png";
            File.WriteAllBytes(fileName, Raw);
            FileName = fileName;
            return fileName;
        }
        public void RemoveTempFile()
        {
            if(File.Exists(FileName))
                File.Delete(FileName);
        }
    }
}

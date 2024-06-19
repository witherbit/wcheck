using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DocxEngine;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.WPF;
using pwither.formatter;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using wcheck.Statistic.Items;
using wcheck.Statistic.Styles;
using DocumentFormat.OpenXml.Wordprocessing;
using wcheck.wcontrols;

namespace wcheck.Statistic.Nodes
{
    [BitSerializable]
    public class PieStatisticNode : IStatisticNode
    {
        public List<PieItem> Pies { get; }
        public ImageNodeStyle ImageStyle { get; set; }

        public PieStatisticNode()
        {
            ImageStyle = new ImageNodeStyle();
            Pies = new List<PieItem>();
        }
        public PieStatisticNode(List<PieItem> pies)
        {
            ImageStyle = new ImageNodeStyle();
            Pies = pies;
        }
        public PieStatisticNode(ImageNodeStyle style)
        {
            ImageStyle = style;
            Pies = new List<PieItem>();
        }
        public PieStatisticNode(List<PieItem> pies, ImageNodeStyle style)
        {
            ImageStyle = style;
            Pies = pies;
        }

        public string GetCSV()
        {
            return null;
        }

        public void InsertDocx(Docx doc)
        {
            var series = new List<ISeries>();
            foreach(var pie in Pies)
            {
                var s = new PieSeries<int>
                {
                    Values = pie.Values,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 12,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer,
                    DataLabelsFormatter = point => $"{pie.Name} : {point.PrimaryValue.ToString("N2")}",
                };

                if (pie.HexColor != null)
                    s.Fill = GetColorFromHex(pie.HexColor);

                series.Add(s);
            }
            var chart = new PieChart
            {
                Height = 400,
                Width = 400,
                Series = series,
                FontFamily = new System.Windows.Media.FontFamily("Arial"),
                SnapsToDevicePixels = true,
                UseLayoutRounding = true,

            };
            var skChart = new SKPieChart(chart) { Width = 400, Height = 400, DrawMargin = new LiveChartsCore.Measure.Margin(5) };
            var image = skChart.GetImage().ToBitmap();
            var imageNode = new ImageStatisticNode(new ImageItem(ConvertBitmapToBytes(image), ShellHost.Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToTemp)), ImageStyle);
            imageNode.InsertDocx(doc);
        }

        public void Render(StackPanel panel)
        {
            var series = new List<ISeries>();
            foreach (var pie in Pies)
            {
                var s = new PieSeries<int>
                {
                    Values = pie.Values,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 12,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{pie.Name} : {point.PrimaryValue.ToString("N2")}",
                };

                if (pie.HexColor != null)
                    s.Fill = GetColorFromHex(pie.HexColor);

                series.Add(s);
            }
            var chart = new PieChart
            {
                Height = 400,
                Width = 400,
                Series = series,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Arial"),
                SnapsToDevicePixels = true,
                UseLayoutRounding = true,
                
            };
            panel.Children.Add(chart);
        }

        private byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private SolidColorPaint GetColorFromHex(string hex)
        {
            var color = hex.GetColor();
            return new SolidColorPaint(new SKColor(color.R, color.G, color.B, color.A));
        }
    }
}

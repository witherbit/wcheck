using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Win32;
using pwither.formatter;
using pwither.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wcheck.Statistic;
using wcheck.Statistic.Enums;
using wcheck.Statistic.Items;
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Styles;
using wcheck.Statistic.Templates;
using wcheck.wcontrols;

namespace wcheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для StatisticPage.xaml
    /// </summary>
    public partial class StatisticPage : Page
    {
        public StatisticEngine StatisticEngine { get; private set; }
        public StatisticPage(StatisticEngine statisticEngine)
        {
            InitializeComponent();
            StatisticEngine = statisticEngine;
            Title = $"Отчет №{StatisticEngine.StatisticId}";
            StartTask();
        }


        private async void StartTask()
        {
            await Task.Run(() => 
            {
                this.Invoke(() =>
                {
                    StatisticEngine.Render(uiPanel);
                    Task.Delay(1000).Wait();
                    uiCaption.Visibility = Visibility.Collapsed;
                });
            });
        }
        private void uiCloseTab_Click(object sender, RoutedEventArgs e)
        {
            ShellHost.ClosePage(this);
        }

        private void uiSave_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#fca577".GetBrush();
        }

        private void uiSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#fc8343".GetBrush();
        }

        private void uiSave_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#ffffff".GetBrush();
        }

        private void uiSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#fca577".GetBrush();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".wstat";
            dlg.FileName = $"{StatisticEngine.StatisticId}";
            dlg.Filter = "wcheck statistic Files (*.wstat)|*.wstat|Docx Files (.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                string extension = System.IO.Path.GetExtension(filename);
                if(extension == ".docx")
                {
                    try
                    {
                        StatisticEngine.ExportDocx(filename);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить файл:\r\nФайл занят или используется другим процессом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    var bytes = BitSerializer.SerializeNative(StatisticEngine,
                            typeof(StatisticEngine),
                            typeof(TextStatisticNode),
                            typeof(TextAligment),
                            typeof(CeilItem),
                            typeof(ImageItem),
                            typeof(PieItem),
                            typeof(BreakStatisticNode),
                            typeof(ImageStatisticNode),
                            typeof(PieStatisticNode),
                            typeof(TableStatisticNode),
                            typeof(TextStatisticNode),
                            typeof(ImageNodeStyle),
                            typeof(TableNodeStyle),
                            typeof(TextNodeStyle),
                            typeof(IStatisticNode),
                            typeof(IStatisticTemplate),
                            typeof(List<IStatisticNode>),
                            typeof(NetHandleStatisticTemplate),
                            typeof(WpfThinkness));
                    try
                    {
                        System.IO.File.WriteAllBytes(filename, bytes);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить файл:\r\nФайл занят или используется другим процессом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}

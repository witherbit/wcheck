using MdXaml;
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

namespace wcheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для MarkdownPage.xaml
    /// </summary>
    public partial class MarkdownPage : Page
    {
        public MarkdownPage(string title, string md)
        {
            InitializeComponent();
            Title = $"[MD] {title}";
            uiContent.Markdown = md;
        }
    }
}

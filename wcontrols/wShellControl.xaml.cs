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
using wshell.Objects;

namespace wcheck.wcontrols
{
    /// <summary>
    /// Логика взаимодействия для wShellControl.xaml
    /// </summary>
    public partial class wShellControl : UserControl
    {
        public wShellControl(ShellInfo info)
        {
            InitializeComponent();
            uiName.Text = info.Name;
            uiDescription.Text = info.Description;
            uiId.Text = info.Id.ToString();
            uiVersion.Text = info.Version.ToString();
        }
    }
}

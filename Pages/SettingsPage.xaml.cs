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
using wcheck.wshell.Objects;

namespace wcheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public MainSettings Settings { get => ShellHost.Settings; }
        public SettingsPage()
        {
            InitializeComponent();
            uiPropPathXSD.WText = Settings.Get<string>(Consts.ParameterPath.p_PathToXds);
            uiPropPathTemp.WText = Settings.Get<string>(Consts.ParameterPath.p_PathToTemp);
            uiPropPathLog.WText = Settings.Get<string>(Consts.ParameterPath.p_PathToLog);
            uiPropPathShell.WText = Settings.Get<string>(Consts.ParameterPath.p_PathToShell);
            uiPropPathShellSha.WText = Settings.Get<string>(Consts.ParameterPath.p_PathToShellSha);
            uiPropShellAccepted.WText = Settings.Get<string>(Consts.ParameterShell.p_AcceptedShell);
            uiPropShellSha.WChecked = Settings.Get<bool>(Consts.ParameterShell.p_CheckShell);
            uiPropConType.WSelectedIndex = Settings.Get<int>(Consts.ParameterConnection.p_Type);
            uiPropConUseEnc.WChecked = Settings.Get<bool>(Consts.ParameterConnection.p_UseEnc);
            uiPropConPort.WText = Settings.Get<int>(Consts.ParameterConnection.p_Port).ToString();
        }
    }
}

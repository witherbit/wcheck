using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using wcheck.wcontrols;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Objects;
using wshell.Enums;

namespace wcheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public ShellSettings Settings { get => ShellHost.Settings; }
        public ShellBase[] Shells { get; }
        public SettingsPage()
        {
            InitializeComponent();
            uiPropPathXSD.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToXds);
            uiPropPathTemp.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToTemp);
            uiPropPathLog.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToLog);
            uiPropPathShell.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToShell);
            uiPropPathShellDeps.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToDeps);
            uiPropPathShellSha.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToShellSha);
            uiPropShellAccepted.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterShell.p_AcceptedShell);
            uiPropShellSha.WChecked = Settings.GetValue<bool>(SettingsParamConsts.ParameterShell.p_CheckShell);
            uiPropConType.WSelectedIndex = Settings.GetValue<int>(SettingsParamConsts.ParameterConnection.p_Type);
            uiPropConEncKey.WText = Settings.GetValue<string>(SettingsParamConsts.ParameterConnection.p_EncPath);
            uiPropConUseEnc.WChecked = Settings.GetValue<bool>(SettingsParamConsts.ParameterConnection.p_UseEnc);
            uiPropConPort.WText = Settings.GetValue<int>(SettingsParamConsts.ParameterConnection.p_Port).ToString();

            Shells = ShellHost.Instance.Controller.Shells;

            foreach(var shell in Shells)
            {
                AddNewExpander(shell);
            }
        }

        private void uiCloseTab_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сохранить внесенные в настройки изменения?", "Сохранение настроек", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if(result == MessageBoxResult.No)
                ShellHost.ClosePage(this);
            else if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var port = int.Parse(uiPropConPort.WText);
                    if (port < 0 || port > 65535)
                    {
                        MessageBox.Show("Невозможно сохранить настройки:\r\nНеправильно введен параметр \'Порт сервиса\' - порт должен находиться в диапазоне 0-65535", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Settings.Get(SettingsParamConsts.ParameterConnection.p_Port).Value = port;
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить настройки:\r\nНеправильно введен параметр \'Порт сервиса\' - порт может быть записан только числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    FileCheckExist(SettingsParamConsts.ParameterPath.p_PathToXds, uiPropPathXSD);
                    FileCheckExist(SettingsParamConsts.ParameterPath.p_PathToTemp, uiPropPathTemp);
                    FileCheckExist(SettingsParamConsts.ParameterPath.p_PathToLog, uiPropPathLog);
                    FileCheckExist(SettingsParamConsts.ParameterPath.p_PathToShell, uiPropPathShell);
                    FileCheckExist(SettingsParamConsts.ParameterPath.p_PathToDeps,uiPropPathShellDeps);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                

                Settings.Get(SettingsParamConsts.ParameterPath.p_PathToShellSha).Value = uiPropPathShellSha.WText;
                Settings.Get(SettingsParamConsts.ParameterShell.p_AcceptedShell).Value = uiPropShellAccepted.WText;
                Settings.Get(SettingsParamConsts.ParameterShell.p_CheckShell).Value = uiPropShellSha.WChecked;
                Settings.Get(SettingsParamConsts.ParameterConnection.p_Type).Value = uiPropConType.WSelectedIndex;
                Settings.Get(SettingsParamConsts.ParameterConnection.p_EncPath).Value = uiPropConEncKey.WText;
                Settings.Get(SettingsParamConsts.ParameterConnection.p_UseEnc).Value = uiPropConUseEnc.WChecked;

                Settings.Save();
                ShellHost.ClosePage(this);
            }
        }


        private void FileCheckExist(string param, WProperty prop)
        {
            if (!Directory.Exists(prop.WText))
                throw new Exception($"Невозможно сохранить настройки:\r\nПараметр '{prop.Text}' указывает путь к несуществующему объекту - {prop.WText}");
            else
                Settings.Get(param).Value = prop.WText;
        }

        private void AddNewExpander(ShellBase shell)
        {
            var header = new TextBlock
            {
                Foreground = "#1f1f1f".GetBrush(),
                FontFamily = new FontFamily("Arial"),
                Text = $"{GetType(shell.ShellInfo)}{shell.ShellInfo.Name}",
            };
            var expander = new Expander
            {
                Header = header,
            };
            var stackPanel = new StackPanel();
            for(int i = 0; i < shell.Settings.Settings.Count; i++)
            {
                var thk = new Thickness(5);
                if (i > 0) thk = new Thickness(5, 0, 5, 5);
                AddSettingToExpander(shell, stackPanel, shell.Settings.Settings[i], thk);
            }
            expander.Content = stackPanel;
            uiSettingsPanel.Children.Add(expander);
        }

        private void AddSettingToExpander(ShellBase shell, StackPanel panel, SettingsObject obj, Thickness thickness)
        {
            var prop = new WProperty();
            if (obj.Type == typeof(bool))
            {
                prop.WPropertyType = PropertyType.CheckBox;
                prop.Text = obj.ViewName;
                prop.WChecked = (bool)obj.Value;
                prop.Margin = thickness;
            }
            else if (obj.Type == typeof(string))
            {
                prop.WPropertyType = PropertyType.TextBox;
                prop.Text = obj.ViewName;
                prop.WPlaceholder = obj.ViewAdditional;
                prop.WWrap = TextWrapping.Wrap;
                prop.WText = obj.Value as string;
                prop.Margin = thickness;
            }
            if (obj.Type == typeof(int))
            {
                prop.WPropertyType = PropertyType.ComboBox;
                prop.Text = obj.ViewName;
                var spt = (obj.ViewAdditional as string).Split(';');
                foreach (var sp in spt)
                {
                    prop.ComboBoxItems.Add(new ComboBoxItem
                    {
                        Content = sp,
                    });
                }
                prop.WSelectedIndex = (int)obj.Value;
                prop.Margin = thickness;
            }
            prop.WPropertyChanged += ((sender, e) => shell.OnSettingsEdit(obj, e));
            panel.Children.Add(prop);
        }
        private string GetType(ShellInfo info)
        {
            if (info.Type == ShellType.Background)
                return "Модуль работы в фоне: ";
            else if (info.Type == ShellType.Task)
                return "Модуль задачи: ";
            else if (info.Type == ShellType.TaskView)
                return "Модуль визуального компонента задачи: ";
            else if (info.Type == ShellType.View)
                return "Модуль визуального компонента: ";
            return "Неизвестный модуль: ";
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using wcheck.Documents;
using wcheck.Pages;
using wcheck.wshell.Objects;
using wshell.Abstract;

namespace wcheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShellHost _shellHost {  get; set; }
        public MainWindow()
        {
            InitializeComponent();          
            SourceInitialized += OnSourceInitialized;
            Closed += OnClosedHandle;
            var mainPage = new WelcomePage();
            _shellHost = new ShellHost(this, new List<SettingsObject>
            {
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterPath.p_PathToXds,
                    Value = SettingsParamConsts.ParameterPath.v_PathToXds,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterPath.p_PathToTemp,
                    Value = SettingsParamConsts.ParameterPath.v_PathToTemp,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterPath.p_PathToLog,
                    Value = SettingsParamConsts.ParameterPath.v_PathToLog,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterPath.p_PathToShell,
                    Value = SettingsParamConsts.ParameterPath.v_PathToShell,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterPath.p_PathToDeps,
                    Value = SettingsParamConsts.ParameterPath.v_PathToDeps,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterPath.p_PathToShellSha,
                    Value = SettingsParamConsts.ParameterPath.v_PathToShellSha,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterShell.p_AcceptedShell,
                    Value = SettingsParamConsts.ParameterShell.v_AcceptedShell,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterShell.p_CheckShell,
                    Value = SettingsParamConsts.ParameterShell.v_CheckShell,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterConnection.p_Type,
                    Value = SettingsParamConsts.ParameterConnection.v_Type,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterConnection.p_EncPath,
                    Value = string.Empty,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterConnection.p_UseEnc,
                    Value = SettingsParamConsts.ParameterConnection.v_UseEnc,
                },
                new SettingsObject
                {
                    Name = SettingsParamConsts.ParameterConnection.p_Port,
                    Value = SettingsParamConsts.ParameterConnection.v_Port,
                },
            });
            ShellHost.AddPage(mainPage);
        }

        private void OnClosedHandle(object? sender, EventArgs e)
        {
            if(_shellHost != null)
            {
                _shellHost.ContractStore.RevokeRange(_shellHost.Controller.Shells);
            }
        }

        private void OnSourceInitialized(object? sender, EventArgs e)
        {
            var source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeHelpers.WM_NCHITTEST:
                    if (NativeHelpers.IsSnapLayoutEnabled())
                    {
                        // Return HTMAXBUTTON when the mouse is over the maximize/restore button
                        var point = PointFromScreen(new Point(lParam.ToInt32() & 0xFFFF, lParam.ToInt32() >> 16));
                        if (WpfHelpers.GetElementBoundsRelativeToWindow(maximizeRestoreButton, this).Contains(point))
                        {
                            handled = true;
                            // Apply hover button style
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonHoverBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonHoverForeground"];
                            return new IntPtr(NativeHelpers.HTMAXBUTTON);
                        } else
                        {
                            // Apply default button style (cursor is not on the button)
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonForeground"];
                        }
                    }
                    break;
                case NativeHelpers.WM_NCLBUTTONDOWN:
                    if (NativeHelpers.IsSnapLayoutEnabled())
                    {
                        if (wParam.ToInt32() == NativeHelpers.HTMAXBUTTON)
                        {
                            handled = true;
                            // Apply pressed button style
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonPressedBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonPressedForeground"];
                        }
                    }
                    break;
                case NativeHelpers.WM_NCLBUTTONUP:
                    if (NativeHelpers.IsSnapLayoutEnabled())
                    {
                        if (wParam.ToInt32() == NativeHelpers.HTMAXBUTTON)
                        { 
                            // Apply default button style
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonForeground"];
                            // Maximize or restore the window
                            ToggleWindowState();
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }        

        private void maximizeRestoreButton_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            maximizeRestoreButton.ToolTip = WindowState == WindowState.Normal ? "Maximize" : "Restore";
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        public void ToggleWindowState()
        {
            if (WindowState == WindowState.Maximized)
            {
                uiMenuItem_2x0.Header = "Развернуть на весь экран";
                SystemCommands.RestoreWindow(this);
            }
            else
            {
                uiMenuItem_2x0.Header = "Вернуть в исходное состояние";
                SystemCommands.MaximizeWindow(this);
            }
        }

        public void ShowSystemMenu(Point point)
        {
            // Increment coordinates to allow double-click
            ++point.X;
            ++point.Y;
            if (WindowState == WindowState.Normal)
            {
                point.X += Left;
                point.Y += Top;
            }
            SystemCommands.ShowSystemMenu(this, point);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            //0 main
            if (item == uiMenuItem_0x4) this.Close();
            else if (item == uiMenuItem_0x3)
            {
                ShellHost.AddPage(new SettingsPage());
            }
            else if (item == uiMenuItem_0x2) ;
            else if (item == uiMenuItem_0x1) ;
            else if (item == uiMenuItem_0x0) ;

            //1 modules
            else if (item == uiMenuItem_1x1) ;
            else if (item == uiMenuItem_1x0) ;

            //2 view
            else if (item == uiMenuItem_2x1) SystemCommands.MinimizeWindow(this);
            else if (item == uiMenuItem_2x0) ToggleWindowState();

            //3 license
            //license abouts
            else if (item == uiMenuItem_3x1) ;
            //update license data
            else if (item == uiMenuItem_3x0)
            {
                ShellHost.AddPage(new LicenseUpdatePage());
            } 

            //4 help
            else if (item == uiMenuItem_4x3)
            {
                ShellHost.AddPage(new AboutsPage());
            }
            else if (item == uiMenuItem_4x2) ;
            else if (item == uiMenuItem_4x1)
            {
                ShellHost.AddPage(new MarkdownPage("Документация разработчика", "wcheck.Documents.WShell.md".GetEmbeddedResource()));
            }
            else if (item == uiMenuItem_4x0) ;
        }

        private void uiTabController_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(var item in uiTabController.Items)
            {
                var tab = item as TabItem;
                if (tab == uiTabController.SelectedItem)
                    ShellHost.ChangeColor(true, tab);
                else
                    ShellHost.ChangeColor(false, tab);
            }
        }
    }
}

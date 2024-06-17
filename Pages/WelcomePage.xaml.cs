using Microsoft.VisualBasic.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
using wcheck.Utils;
using wcheck.wcontrols;

namespace wcheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
            Logger.LogWrite += OnLogWrite;
            uiTextBoxIp.Text = GetIp();
            uiTextBoxOS.Text = GetOS();
            uiTextBoxVersion.Text = GetVersion();
            uiTextBoxMachineName.Text = GetName();
            uiTextBoxUser.Text = GetUser();
        }

        private void OnLogWrite(object? sender, LogContent e)
        {
            var log = string.Empty;
            if (e.Exception != null)
                log = $"[{DateTime.Now}] [{e.Type}] from {e.Sender} >: {e.Message}\n\tException: {e.Exception.ToString()}";
            else
                log = $"[{DateTime.Now}] [{e.Type}] from {e.Sender} >: {e.Message}";
            var brush = "#1f1f1f".GetBrush();
            if (e.Type == LogType.WARN) brush = "#ff9900".GetBrush();
            else if (e.Type == LogType.CRITICAL) brush = "#ff3900".GetBrush();

            if(uiStackPanelLogs.Children.Count > 500)
                uiStackPanelLogs.Children.RemoveAt(0);

            uiStackPanelLogs.Children.Add(new TextBox
            {
                FontFamily = new FontFamily("Arial"),
                Text = log,
                Foreground = brush,
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 10)
            });
            //uiScrollLogs.ScrollToEnd();
        }

        private static string GetIp()
        {
            string result = "?";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {

                var defGate = from nics in NetworkInterface.GetAllNetworkInterfaces()


                                     from props in nics.GetIPProperties().GatewayAddresses
                                     where nics.OperationalStatus == OperationalStatus.Up
                                     select props.Address.ToString(); // this sets the default gateway in a variable

                GatewayIPAddressInformationCollection prop = ni.GetIPProperties().GatewayAddresses;

                if (defGate.First() != null)
                {

                    IPInterfaceProperties ipProps = ni.GetIPProperties();

                    foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                    {

                        if (addr.Address.ToString().Contains(defGate.First().Remove(defGate.First().LastIndexOf(".")))) // The IP address of the computer is always a bit equal to the default gateway except for the last group of numbers. This splits it and checks if the ip without the last group matches the default gateway
                        {

                            if (result == "?") // check if the string has been changed before
                            {
                                result = addr.Address.ToString(); // put the ip address in a string that you can use.
                            }
                        }

                    }

                }

            }
            return result;
        }
        private static string GetOS()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
        private static string GetVersion()
        {
            return Environment.OSVersion.Version.ToString();
        }
        private static string GetName()
        {
            return Environment.MachineName.ToString();
        }
        private static string GetUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        internal static string SysInfoGet()
        {
            return $"Open app from {GetUser()} on {GetName()}:\r\n\tIp:{GetIp()}\r\n\tOS:{GetOS() + " " + GetVersion()}";
        }

        private void uiTextBoxIp_MouseEnter(object sender, MouseEventArgs e)
        {
            uiTextBoxIp.Foreground = "#663dfc".GetBrush();
        }

        private void uiTextBoxIp_MouseLeave(object sender, MouseEventArgs e)
        {
            uiTextBoxIp.Foreground = "#1f1f1f".GetBrush();
        }

        private void uiTextBoxIp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            uiTextBoxIp.Foreground = "#663dfc".GetBrush();
            Clipboard.SetText(uiTextBoxIp.Text);
        }

        private void uiTextBoxIp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            uiTextBoxIp.Foreground = "#4428a8".GetBrush();
        }
    }
}

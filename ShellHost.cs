using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using wcheck.Pages;
using wcheck.wcontrols;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Core;

namespace wcheck
{
    internal class ShellHost : IHost
    {
        public static MainSettings Settings { get; set; }
        public MainWindow HostWindow { get; private set; }
        public static ShellHost Instance { get; private set; }
        public ShellCallback Callback { get; private set; }
        public ShellController Controller { get; private set; }

        public TabablzControl Tab => HostWindow.uiTabController;

        public ContractStore ContractStore => Controller.ContractStore;

        public static List<TabItem> Items { get; private set; }

        public ShellHost(MainWindow mainWindow, List<SettingsObject> defaultSettings)
        {
            Settings = MainSettings.Load(defaultSettings);
            Instance = this;
            HostWindow = mainWindow;
            Callback = new ShellCallback();
            Controller = new ShellController();
            Items = new List<TabItem>();
        }

        private static Border GetBorderTab(string header)
        {
            var border = new Border
            {
                Child = new TextBlock { Text = header, Margin = new Thickness(5), FontFamily = new FontFamily("Arial"), Foreground = "#ffffff".GetBrush() },
                Background = "#3e3e3e".GetBrush(),
                CornerRadius = new CornerRadius(0)
            };

            return border;
        }

        public static void ChangeColor(bool enable, TabItem item)
        {
            var tabObjects = GetObjectsFromHeader(item);
            if (!enable)
            {
                tabObjects.Border.Background = "#1f1f1f".GetBrush();
                tabObjects.TextBlock.Foreground = "#ffffff".GetBrush();
            }
            else
            {
                tabObjects.Border.Background = "#fca577".GetBrush();
                tabObjects.TextBlock.Foreground = "#1f1f1f".GetBrush();
            }
        }

        public static (Border Border, TextBlock TextBlock, Page page) GetObjectsFromHeader(TabItem item)
        {
            var border = item.Header as Border;
            var textBlock = border.Child as TextBlock;
            return (border, textBlock, item.Content as Page);
        }

        public static void AddPage(Page page, string title = null)
        {
            if (Items.FirstOrDefault(x => ((x.Header as Border).Child as TextBlock).Text == page.Title) != null)
            {
                MessageBox.Show("Вы уже открыли эту страницу", "Ошибка");
                return;
            }
            var tab = new TabItem
            {
                Header = GetBorderTab(title.IsNullOrEmpty() ? page.Title : title),
                Content = new Frame
                {
                    NavigationUIVisibility = NavigationUIVisibility.Hidden,
                    Content = page
                },
                IsSelected = true
            };
            Instance.Tab.Items.Add(tab);
            Items.Add(tab);
        }

        public static void ClosePage(Page page, Action? action = null)
        {
            var item = Items.FirstOrDefault(x => ((x.Header as Border).Child as TextBlock).Text == page.Title);
            if (item != null)
            {
                action?.Invoke();
                Instance.Tab.Items.Remove(item);
                Items.Remove(item);
            }
        }

        public static T TryFoundResource<T>(string resourceName)
        {
            return (T)Instance.HostWindow.TryFindResource(resourceName);
        }

        public static void Invoke(Action callback)
        {
            Instance.HostWindow.Invoke(callback);
        }
    }
}

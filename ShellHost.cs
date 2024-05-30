using Dragablz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using wcheck.Pages;
using wcheck.Utils;
using wcheck.wcontrols;
using wcheck.wshell.Enums;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Core;
using wshell.Enums;
using wshell.Objects;

namespace wcheck
{
    internal class ShellHost : IHost
    {
        public static ShellSettings Settings { get; set; }
        public MainWindow HostWindow { get; private set; }
        public static ShellHost Instance { get; private set; }
        public ShellCallback Callback { get; private set; }
        public ShellController Controller { get; private set; }

        public TabablzControl Tab => HostWindow.uiTabController;

        public ContractStore ContractStore => Controller.ContractStore;

        public static List<TabItem> Items { get; private set; }

        public ShellHost(MainWindow mainWindow, List<SettingsObject> defaultSettings)
        {
            Settings = ShellSettings.Load("23aca232-542a-4716-b51a-fe050de5fcb9", defaultSettings);
            CreateDir();
            Instance = this;

            Logger.Log(new LogContent(WelcomePage.SysInfoGet(), this));
            Logger.Clear();
            Logger.Log(new LogContent("Cleared older log files", this));

            HostWindow = mainWindow;
            Callback = new ShellCallback();

            Controller = new ShellController();

            Logger.Log(new LogContent("Loading shells", this));
            Controller.LoadAll(Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToShell), Settings.GetValue<string>(SettingsParamConsts.ParameterShell.p_AcceptedShell));
            if (Controller.Shells.Length > 0)
                Controller.RegisterAll(this);

            Logger.Log(new LogContent($"{Controller.Shells.Length} shells registered ,{Controller.DeniedIds.Length} shells ignored", this));

            Callback.Callback += OnCallback;
            Callback.RequestCallback += OnRequestCallback;

            UpdateShellMenu();

            Items = new List<TabItem>();
        }

        private Schema OnRequestCallback(ShellBase shell, Schema schema)
        {
            Logger.Log(new LogContent($"Callback request from {shell.ShellInfo.Id} [{shell.ShellInfo.Name}]\r\nType: {schema.Type}", this));
            switch (schema.Type)
            {
                case CallbackType.ReloadPageRequest:
                    shell.Stop();
                    return new Schema(CallbackType.ReloadPageResponse).SetProviding(schema.GetProviding<Page>());

                case CallbackType.ShellInfosRequest:
                    List<ShellInfo> infs = new List<ShellInfo>();
                    foreach (var shl in Controller.Shells)
                        infs.Add(shl.ShellInfo);
                    return new Schema(CallbackType.ShellInfosResponse).SetProviding(infs.ToArray());

                case CallbackType.GetShellInstanceRequest:
                    return new Schema(CallbackType.GetShellInstanceResponse).SetProviding(Controller.GetShellById(schema.GetProviding<ShellInfo>().Id));

                case CallbackType.SettingsParameterRequest:
                    return new Schema(CallbackType.SettingsParameterResponse).SetProviding(Settings.Get(schema.GetProviding<string>()));

                default:
                    return new Schema(CallbackType.EmptyResponse);
            }
        }

        private void OnCallback(ShellBase shell, Schema schema)
        {
            Logger.Log(new LogContent($"Callback invoke from {shell.ShellInfo.Id} [{shell.ShellInfo.Name}]\r\nType: {schema.Type}", this));
            switch (schema.Type)
            {
                case CallbackType.RegisterPage:
                    AddPage(schema.GetProviding<Page>());
                    break;
                case CallbackType.UnregisterPage:
                    ClosePage(schema.GetProviding<Page>());
                    break;
            }
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
            var item = Items.FirstOrDefault(x => ((x.Header as Border).Child as TextBlock).Text == page.Title);
            if (item != null)
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
            Logger.Log(new LogContent($"Registered the new page", page));
        }

        public static void ClosePage(Page page, Action? action = null)
        {
            var item = Items.FirstOrDefault(x => ((x.Header as Border).Child as TextBlock).Text == page.Title);
            if (item != null)
            {
                action?.Invoke();
                Instance.Tab.Items.Remove(item);
                Items.Remove(item);
                Logger.Log(new LogContent($"Unregister the page", page));
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

        public static Schema InvokeRequest(ShellBase shell, Schema schema)
        {
            var result = Instance.ContractStore.Get(shell);
            return result.Contract.Invoke(schema);
        }

        private void CreateDir()
        {
            Directory.CreateDirectory(Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToXds));
            Directory.CreateDirectory(Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToTemp));
            Directory.CreateDirectory(Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToLog));
            Directory.CreateDirectory(Settings.GetValue<string>(SettingsParamConsts.ParameterPath.p_PathToShell));
        }

        private void UpdateShellMenu()
        {
            var backgrounds = Controller.Shells.Where(x => x.ShellInfo.Type == ShellType.Background).ToList();
            var tasks = Controller.Shells.Where(x => x.ShellInfo.Type == ShellType.Task).ToList();
            var taskViews = Controller.Shells.Where(x => x.ShellInfo.Type == ShellType.TaskView).ToList();
            var views = Controller.Shells.Where(x => x.ShellInfo.Type == ShellType.View).ToList();
            if(backgrounds.Count > 0)
            {
                HostWindow.uiMenuItem_1.Items.Add(new Separator());
                foreach(var item in backgrounds)
                {
                    HostWindow.uiMenuItem_1.Items.Add(new MenuItem
                    {
                        Header = item.ShellInfo.Name + " (Фоновый)",
                    });
                }
            }
            if (tasks.Count > 0)
            {
                HostWindow.uiMenuItem_1.Items.Add(new Separator());
                foreach (var item in tasks)
                {
                    var menuItem = new MenuItem
                    {
                        Header = item.ShellInfo.Name + " (Задача)",
                    };
                    menuItem.Click += (s, e) => { item.Run(); };
                    HostWindow.uiMenuItem_1.Items.Add(menuItem);
                }
            }
            if (taskViews.Count > 0)
            {
                HostWindow.uiMenuItem_1.Items.Add(new Separator());
                foreach (var item in taskViews)
                {
                    HostWindow.uiMenuItem_1.Items.Add(new MenuItem
                    {
                        Header = item.ShellInfo.Name + " (Компонент задачи)",
                    });
                }
            }
            if (views.Count > 0)
            {
                HostWindow.uiMenuItem_1.Items.Add(new Separator());
                foreach (var item in views)
                {
                    HostWindow.uiMenuItem_1.Items.Add(new MenuItem
                    {
                        Header = item.ShellInfo.Name + " (Компонент)",
                    });
                }
            }
        }
    }
}

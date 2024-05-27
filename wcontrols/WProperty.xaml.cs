using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace wcheck.wcontrols
{
    /// <summary>
    /// Логика взаимодействия для WProperty.xaml
    /// </summary>
    public partial class WProperty : UserControl
    {
        public static readonly DependencyProperty ComboBoxItemsProperty = DependencyProperty.Register(
            "ComboBoxItems",
            typeof(ObservableCollection<ComboBoxItem>),
            typeof(WProperty),
            new PropertyMetadata(new ObservableCollection<ComboBoxItem>()));

        public ObservableCollection<ComboBoxItem> ComboBoxItems
        {
            get { return (ObservableCollection<ComboBoxItem>)GetValue(ComboBoxItemsProperty); }
            set { SetValue(ComboBoxItemsProperty, value); }
        }

        public event EventHandler<PropertyEventArgs> WPropertyChanged;
        public PropertyType _propertyType;
        public PropertyType WPropertyType { 
            get => _propertyType; 
            set 
            {
                _propertyType = value;
                switch (value)
                {
                    case PropertyType.TextBox:
                        uiTextBox.Visibility = Visibility.Visible;
                        break;
                    case PropertyType.CheckBox:
                        uiCheckBox.Visibility = Visibility.Visible;
                        break;
                    case PropertyType.ComboBox:
                        uiComboBox.Visibility = Visibility.Visible;
                        break;
                }
            } 
        }

        public string WPlaceholder { get => uiTextBox.Tag as string; set => uiTextBox.Tag = value; }

        public string WText { get => uiTextBox.Text; set => uiTextBox.Text = value; }

        public string Text { get => uiTextInfo.Text; set => uiTextInfo.Text = value; }

        public int WSelectedIndex { get => uiComboBox.SelectedIndex; set => uiComboBox.SelectedIndex = value; }

        public bool? WChecked { get => uiCheckBox.IsChecked; set => uiCheckBox.IsChecked = value; }

        public TextWrapping WWrap
        {
            get => uiTextBox.TextWrapping;
            set
            {
                if(value == TextWrapping.WrapWithOverflow || value == TextWrapping.Wrap)
                    uiTextBox.AcceptsReturn = true;
                else
                    uiTextBox.AcceptsReturn = false;
                uiTextBox.TextWrapping = value;
            }
        }

        public WProperty()
        {
            ComboBoxItems = new ObservableCollection<ComboBoxItem>();
            InitializeComponent();
            uiComboBox.ItemsSource = ComboBoxItems;
        }

        private void uiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (_propertyType != PropertyType.CheckBox) return;
            WPropertyChanged?.Invoke(this, new PropertyEventArgs
            {
                Type = _propertyType,
                Checked = uiCheckBox.IsChecked
            });
        }

        private void uiComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_propertyType != PropertyType.ComboBox) return;
            WPropertyChanged?.Invoke(this, new PropertyEventArgs
            {
                Type = _propertyType,
                ComboBoxItem = uiComboBox.SelectedItem,
                SelectedIndex = uiComboBox.SelectedIndex
            });
        }

        private void uiTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (_propertyType != PropertyType.TextBox) return;
            WPropertyChanged?.Invoke(this, new PropertyEventArgs
            {
                Type = _propertyType,
                Text = uiTextBox.Text,
            });
        }

        private void uiTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_propertyType != PropertyType.TextBox) return;
            WPropertyChanged?.Invoke(this, new PropertyEventArgs
            {
                Type = _propertyType,
                Text = uiTextBox.Text,
            });
        }
    }

    public sealed class PropertyEventArgs
    {
        public PropertyType Type { get; set; }
        public string Text {  get; set; }
        public bool? Checked { get; set; }
        public object ComboBoxItem { get; set; }
        public int SelectedIndex { get; set; }

    }

    public enum PropertyType
    {
        TextBox,
        CheckBox,
        ComboBox
    }
}

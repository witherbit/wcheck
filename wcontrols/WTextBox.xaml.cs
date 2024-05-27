using MaterialDesignThemes.Wpf;
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

namespace wcheck.wcontrols
{
    /// <summary>
    /// Логика взаимодействия для WTextBox.xaml
    /// </summary>
    public partial class WTextBox : UserControl
    {
        private Brush _primaryColor => "#fca577".GetBrush();
        private Brush _foregroundColor => "#1f1f1f".GetBrush();
        private Brush _placeholderColor => "#535353".GetBrush();

        public event EventHandler<TextCompositionEventArgs> TextChange;

        private bool _chk = false;
        public string PlaceHolder
        {
            get => uiPlaceHolder.Text;
            set
            {
                uiPlaceHolder.Text = value;
                CheckPlaceHolder();
            }
        }
        public int MaxLength
        {
            get => uiTextBox.MaxLength;
            set
            {
                uiTextBox.MaxLength = value;
            }
        }
        public bool IsReadOnly { get => uiTextBox.IsReadOnly; set => uiTextBox.IsReadOnly = value; }
        public string Text
        {
            get => uiTextBox.Text;
            set
            {
                uiTextBox.Text = value;
                CheckPlaceHolder();
            }
        }
        public TextAlignment TextAlignment { get => uiTextBox.TextAlignment; set => uiTextBox.TextAlignment = value; }
        public WTextBox()
        {
            InitializeComponent();
            CheckPlaceHolder();
        }

        private void uiTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckPlaceHolder();
            TextChange?.Invoke(this, e);
        }

        private void uiTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            uiTextBox_PreviewTextInput(sender, null);
        }

        private void CheckPlaceHolder()
        {
            double duration = 0.1;
            if (!string.IsNullOrEmpty(uiTextBox.Text) && !_chk)
            {
                _chk = true;
                uiPlaceHolder.MarginAnimation(new Thickness(10, 0, 0, 27), duration);
                uiPlaceHolder.ForegroundFadeTo(_primaryColor, duration);
                uiPlaceHolder.FontSizeAnimation(10, duration);
            }
            else if (string.IsNullOrEmpty(uiTextBox.Text) && _chk)
            {
                _chk = false;
                uiPlaceHolder.MarginAnimation(new Thickness(10, 0, 0, 0), duration);
                uiPlaceHolder.ForegroundFadeTo(_placeholderColor, duration);
                uiPlaceHolder.FontSizeAnimation(uiTextBox.FontSize, duration);
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            uiTextBox.Focus();
        }
    }
}

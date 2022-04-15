using AutomatonBuilder.Entities.Args;
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
using System.Windows.Shapes;

namespace AutomatonBuilder.Modals
{
    /// <summary>
    /// Interaction logic for TextInputWindow.xaml
    /// </summary>
    public partial class TextInputWindow : Window
    {
        public AddTextArgs Input { get; set; }
        public TextInputWindow()
        {
            InitializeComponent();
            this.InputBox.Focus();
            this.InputBox.FontSize = double.Parse(this.FontSizeComboBox.Text);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            TextAlignment textAlignment = TextAlignment.Center;
            switch (this.InputBox.HorizontalContentAlignment)
            {
                case HorizontalAlignment.Left:
                    textAlignment = TextAlignment.Left;
                    break;
                case HorizontalAlignment.Right:
                    textAlignment = TextAlignment.Right;
                    break;
                default:
                    break;
            }

            this.Input = new AddTextArgs
            {
                Text = this.InputBox.Text,
                Color = this.InputBox.Foreground,
                FontSize = this.InputBox.FontSize,
                Alignment = textAlignment,
                Style = this.InputBox.FontWeight,
            };
            this.DialogResult = true;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.IsKeyDown(Key.RightShift))
                {
                    this.InputBox.Text += "\n";
                    this.InputBox.SelectionStart = this.InputBox.Text.Length;
                }
                else
                {
                    AddBtn_Click(null, null);
                }
            }

            if (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Key == Key.OemPlus)
                {
                    this.FontSizeComboBox.SelectedIndex = Math.Min(this.FontSizeComboBox.SelectedIndex + 1, this.FontSizeComboBox.Items.Count);
                }
                if (e.Key == Key.OemMinus)
                {
                    this.FontSizeComboBox.SelectedIndex = Math.Max(this.FontSizeComboBox.SelectedIndex - 1, 0);
                }
                if (e.Key == Key.E)
                {
                    AlignCenterToggle_Click(null, null);

                }
                if (e.Key == Key.R)
                {
                    AlignRightToggle_Click(null, null);

                }
                if (e.Key == Key.L)
                {
                    AlignLeftToggle_Click(null, null);
                }
                if (e.Key == Key.B)
                {
                    BoldToggle_Click(null, null);
                    this.BoldToggle.IsChecked = !this.BoldToggle.IsChecked.Value;
                }

            }

            if (e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InputBox.Focus();
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (double.TryParse(((ComboBoxItem)this.FontSizeComboBox.SelectedValue).Content?.ToString(), out double result))
                this.InputBox.FontSize = result;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            this.InputBox.Foreground = new SolidColorBrush(e.NewValue.GetValueOrDefault());
        }

        private void BoldToggle_Click(object sender, RoutedEventArgs e)
        {
            this.InputBox.FontWeight = FontWeight.FromOpenTypeWeight(1100 - this.InputBox.FontWeight.ToOpenTypeWeight());
        }

        private void AlignLeftToggle_Click(object sender, RoutedEventArgs e)
        {
            this.InputBox.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.AlignCenterToggle.IsChecked = false;
            this.AlignRightToggle.IsChecked = false;
            this.AlignLeftToggle.IsChecked = true;
        }

        private void AlignCenterToggle_Click(object sender, RoutedEventArgs e)
        {
            this.InputBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.AlignLeftToggle.IsChecked = false;
            this.AlignRightToggle.IsChecked = false;
            this.AlignCenterToggle.IsChecked = true;
        }

        private void AlignRightToggle_Click(object sender, RoutedEventArgs e)
        {
            this.InputBox.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.AlignLeftToggle.IsChecked = false;
            this.AlignRightToggle.IsChecked = true;
            this.AlignCenterToggle.IsChecked = false;
        }
    }
}

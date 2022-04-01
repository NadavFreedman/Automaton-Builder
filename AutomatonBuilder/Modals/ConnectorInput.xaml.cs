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
    /// Interaction logic for ConnectorInput.xaml
    /// </summary>
    public partial class ConnectorInput : Window
    {
        public string Input { get; set; }
        public ConnectorInput(string from, string to)
        {
            InitializeComponent();
            InstructionsBlock.Text = string.Format("Connect Between {0} to {1}:", from, to);
        }

        public ConnectorInput(string instructions)
        {
            InitializeComponent();
            InstructionsBlock.Text = instructions;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Input = this.InputBox.Text;
            this.DialogResult = true;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddBtn_Click(null, null);
            if (e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InputBox.Focus();
        }
    }
}

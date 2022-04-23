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
    /// Interaction logic for RunWordModal.xaml
    /// </summary>
    public partial class RunWordModal : Window
    {
        public string EnteredWord { get; set; }
        public RunWordModal()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InputBox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
            if (e.Key == Key.Enter)
                RunBtn_Click(null, null);
        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            this.EnteredWord = this.InputBox.Text;
            DialogResult = true;
        }
    }
}

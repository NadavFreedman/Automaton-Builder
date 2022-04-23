using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Pages;
using AutomatonBuilder.Utils;
using Microsoft.Win32;
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

namespace AutomatonBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.GoToMenu();
        }

        public void NewProject(AutomatonTypes selectedType)
        {
            this.WindowState = WindowState.Maximized;
            this.Content = new MainEditingScreen(this, selectedType);
        }

        public void OpenProject(string filePath)
        {
            this.Title = filePath.Split(@"\").Last().Split(".").First();
            var screen = new MainEditingScreen(this);
            screen.context = SavingUtils.LoadContextFromFile(screen, filePath);
            this.WindowState = WindowState.Maximized;
            this.Content = screen;
        }

        public void GoToMenu()
        {
            Page page = new MainSelectionWindow(this);
            this.WindowState = WindowState.Normal;
            this.Content = page;
        }
    }
}

using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Entities.Enums;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutomatonBuilder.Pages
{
    /// <summary>
    /// Interaction logic for MainSelectionWindow.xaml
    /// </summary>
    public partial class MainSelectionWindow : Page
    {
        private readonly MainWindow host;
        private Canvas showingPreviewCanvas;
        private readonly Dictionary<SelectionPageOptions, Canvas> previewCanvases;

        public MainSelectionWindow(MainWindow host)
        {
            InitializeComponent();

            this.previewCanvases = new Dictionary<SelectionPageOptions, Canvas>();
            foreach (SelectionPageOptions option in (SelectionPageOptions[])Enum.GetValues(typeof(SelectionPageOptions)))
            {
                this.previewCanvases[option] = PreviewScreenFactory.CreatePreviewCanvasByOption(option);
                Grid.SetColumn(this.previewCanvases[option], 3);
                Grid.SetColumnSpan(this.previewCanvases[option], 3);
                Grid.SetRow(this.previewCanvases[option], 2);
                Grid.SetRowSpan(this.previewCanvases[option], 7);
            }

            this.host = host;
        }

        private void OpenProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Editable Automaton Files (.eaf)|*.eaf",
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                this.host.OpenProject(openFileDialog.FileName);
            }
        }

        private void NewBasicBtn_Click(object sender, RoutedEventArgs e)
        {
            this.host.NewProject(AutomatonTypes.Basic);
            this.host.Title = "BasicAutomaton";
        }

        private void NewPushdownBtn_Click(object sender, RoutedEventArgs e)
        {
            this.host.NewProject(AutomatonTypes.Pushdown);
            this.host.Title = "PushdownAutomaton";
        }

        private void NewTuringBtn_Click(object sender, RoutedEventArgs e)
        {
            this.host.NewProject(AutomatonTypes.Turing);
            this.host.Title = "TuringAutomaton";
        }

        private void Button_MouseHover(object sender, MouseEventArgs e)
        {
            if (sender == OpenProjectBtn)
                this.showingPreviewCanvas = this.previewCanvases[SelectionPageOptions.Open];
            else if (sender == NewBasicBtn)
                this.showingPreviewCanvas = this.previewCanvases[SelectionPageOptions.Basic];
            else if (sender == NewPushdownBtn)
                this.showingPreviewCanvas = this.previewCanvases[SelectionPageOptions.Pushdown];
            else if (sender == NewTuringBtn)
                this.showingPreviewCanvas = this.previewCanvases[SelectionPageOptions.Turing];

            this.MainGrid.Children.Add(showingPreviewCanvas);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            this.MainGrid.Children.Remove(showingPreviewCanvas);
        }
    }
}

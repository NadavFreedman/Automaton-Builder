using AutomatonBuilder.Entities.Connectors.ConnectorData;
using AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Interfaces;
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

namespace AutomatonBuilder.Modals.ConnectionModals
{
    /// <summary>
    /// Interaction logic for TuringConnectorModal.xaml
    /// </summary>
    public partial class TuringConnectorModal : Window, IConnectionModal
    {
        public IConnectorData? ConnectorData { get; set; }
        private readonly List<SingleTuringData> singleDataList;
        private TuringActions selectedAction;

        public TuringConnectorModal(string from, string to)
        {
            InitializeComponent();
            singleDataList = new List<SingleTuringData>();
            InstructionsBlock.Text = string.Format("Connect Between {0} to {1}:", from, to);
        }

        private void ReadValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TuringInput(this.ReadValueBox);
            this.ToggleAddBtn();
            this.WriteActionBox.Focus();
        }

        private void WriteActionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TuringInput(this.WriteActionBox);
            this.ToggleAddBtn();
            this.ActionTuringkBox.Focus();
        }

        private void ActionStackBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                this.ActionTuringkBox.Text = "🡆";
                this.selectedAction = TuringActions.MoveRight;
                this.ToggleAddBtn();
            }
            else if (e.Key == Key.Left)
            {
                this.ActionTuringkBox.Text = "🡄";
                this.selectedAction = TuringActions.MoveLeft;
                this.ToggleAddBtn();
            }
        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.AddBtn.IsEnabled)
            {
                var singleData = new SingleTuringData(this.ReadValueBox.Text.First(),
                    this.WriteActionBox.Text.First(),
                    this.selectedAction);
                InsertNewEntryToStack(singleData);
                this.ReadValueBox.Clear();
                this.WriteActionBox.Clear();
                this.ActionTuringkBox.Clear();
                this.AddBtn.IsEnabled = false;
                this.ToggleDoneBtn();
            }
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.DoneBtn.IsEnabled)
            {
                this.ConnectorData = new TuringAutomatonData(singleDataList);
                this.DialogResult = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ReadValueBox.Focus();
        }

        private void InsertNewEntryToStack(SingleTuringData newEntry)
        {
            if (this.singleDataList.Any(connection => connection.ToString() == newEntry.ToString())) return;

            this.singleDataList.Add(newEntry);

            DockPanel row = new();

            TextBlock connectionBlock = new()
            {
                Text = newEntry.ToString(),
            };

            Button discardButton = new()
            {
                Content = "X",
                Width = 15,
                HorizontalAlignment = HorizontalAlignment.Right,
            };


            row.Children.Add(connectionBlock);
            row.Children.Add(discardButton);

            Border border = new()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White,
                Tag = newEntry,
                Child = row
            };

            discardButton.Tag = border;
            discardButton.Click += DiscardButton_Click;

            this.InsertedConnectionsPanel.Children.Insert(0, border);
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            var taggedBorder = (sender as Button)!.Tag as Border;
            var taggedEntry = taggedBorder!.Tag as SingleTuringData;

            this.InsertedConnectionsPanel.Children.Remove(taggedBorder);
            this.singleDataList.Remove(taggedEntry!);

            this.ToggleDoneBtn();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddBtn_Click(null, null);
                this.ReadValueBox.Focus();
                if (!Keyboard.IsKeyDown(Key.RightShift) && !Keyboard.IsKeyDown(Key.LeftShift))
                {
                    DoneBtn_Click(null, null);
                }

            }
        }

        private void ToggleAddBtn()
        {
            bool top = this.WriteActionBox.Text.Length > 0;
            bool word = this.ReadValueBox.Text.Length > 0;
            bool action = this.ActionTuringkBox.Text.Length > 0;

            this.AddBtn.IsEnabled = top && word && action;
            this.DoneBtn.IsEnabled = top && word && action;
        }

        private void ToggleDoneBtn()
        {
            this.DoneBtn.IsEnabled = this.singleDataList.Count > 0;
        }

        private void TuringInput(TextBox inputBox)
        {
            if (inputBox.Text.Length > 1)
            {
                inputBox.Text = this.ReadValueBox.Text.Last().ToString();
            }
            if (inputBox.Text.FirstOrDefault() == '|')
            {
                inputBox.Text = "├";
            }
            if (inputBox.Text.FirstOrDefault() == ' ')
            {
                inputBox.Text = "△";
            }
        }
    }
}

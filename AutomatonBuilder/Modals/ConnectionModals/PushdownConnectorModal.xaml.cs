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
    /// Interaction logic for PushdownConnectorModal.xaml
    /// </summary>
    public partial class PushdownConnectorModal : Window, IConnectionModal
    {
        public IConnectorData? ConnectorData { get; set; }
        private readonly List<SinglePushdownData> singleDataList;
        private PushdownActions selectedAction;

        public PushdownConnectorModal(string from, string to)
        {
            InitializeComponent();
            singleDataList = new List<SinglePushdownData>();
            InstructionsBlock.Text = string.Format("Connect Between {0} to {1}:", from, to);
        }

        private void WordValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.WordValueBox.Text.Length > 1)
            {
                this.WordValueBox.Text = this.WordValueBox.Text.Last().ToString();
            }
            this.ToggleAddBtn();
            this.TopValueBox.Focus();
        }

        private void TopValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.TopValueBox.Text.Length > 1)
            {
                this.TopValueBox.Text = this.TopValueBox.Text.Last().ToString();
            }
            if (this.TopValueBox.Text.FirstOrDefault() == '_')
            {
                this.TopValueBox.Text = "⟂";
            }
            if (this.selectedAction == PushdownActions.Pop)
            {
                this.ActionStackValueBox.Text = this.TopValueBox.Text;
            }

            this.ToggleAddBtn();
            this.ActionStackBox.Focus();
        }

        private void ActionStackBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                this.ActionStackBox.Text = "▲";
                this.selectedAction = PushdownActions.Pop;
                this.ActionStackValueBox.Text = this.TopValueBox.Text;
                this.ActionStackValueBox.IsEnabled = false;
                this.ToggleAddBtn();
            }
            else if (e.Key == Key.Down)
            {
                this.ActionStackBox.Text = "▼";
                this.selectedAction = PushdownActions.Push;
                this.ActionStackValueBox.IsEnabled = true;
                this.ActionStackValueBox.Focus();
                this.ToggleAddBtn();
            }
            else if (e.Key == Key.Space)
            {
                this.ActionStackBox.Text = "NC";
                this.selectedAction = PushdownActions.NoChange;
                this.ActionStackValueBox.Text = "";
                this.ActionStackValueBox.IsEnabled = false;
                this.ToggleAddBtn();
            }
        }

        private void ActionStackValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.TopValueBox.Text.Length > 1)
            {
                this.ActionStackBox.Text = this.ActionStackBox.Text.Last().ToString();
            }

            this.ToggleAddBtn();
            this.ActionStackBox.Focus();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.AddBtn.IsEnabled)
            {
                var singleData = new SinglePushdownData(this.WordValueBox.Text.First(),
                    this.TopValueBox.Text.First(),
                    selectedAction,
                    this.ActionStackValueBox.Text.FirstOrDefault());
                InsertNewEntryToStack(singleData);
                this.WordValueBox.Clear();
                this.TopValueBox.Clear();
                this.ActionStackBox.Clear();
                this.ActionStackValueBox.Clear();
                this.AddBtn.IsEnabled = false;
                this.ToggleDoneBtn();
            }
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.DoneBtn.IsEnabled)
            {
                this.ConnectorData = new PushdownAutomatonData(singleDataList);
                this.DialogResult = true;
            }
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WordValueBox.Focus();
        }

        private void InsertNewEntryToStack(SinglePushdownData newEntry)
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
            var taggedEntry = taggedBorder!.Tag as SinglePushdownData;

            this.InsertedConnectionsPanel.Children.Remove(taggedBorder);
            this.singleDataList.Remove(taggedEntry!);

            this.ToggleDoneBtn();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddBtn_Click(null, null);
                this.WordValueBox.Focus();
                if (!Keyboard.IsKeyDown(Key.RightShift) && !Keyboard.IsKeyDown(Key.LeftShift))
                {
                    DoneBtn_Click(null, null);
                }
            }
        }

        private void ToggleAddBtn()
        {
            bool top = this.TopValueBox.Text.Length == 1;
            bool word = this.WordValueBox.Text.Length == 1;
            bool action = this.ActionStackBox.Text.Length > 0;
            bool actionValue = this.selectedAction == PushdownActions.NoChange || this.ActionStackValueBox.Text.Length == 1;

            this.AddBtn.IsEnabled = top && word && action && actionValue;
            this.DoneBtn.IsEnabled = top && word && action && actionValue;
        }

        private void ToggleDoneBtn()
        {
            this.DoneBtn.IsEnabled = this.singleDataList.Count > 0;
        }
    }
}

using AutomatonBuilder.Entities.Connectors.ConnectorData;
using AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutomatonBuilder.Modals.ConnectionModals
{
    /// <summary>
    /// Interaction logic for ConnectorInput.xaml
    /// </summary>
    public partial class BasicConnectorModal : Window, IConnectionModal
    {
        public IConnectorData? ConnectorData { get; set; }
        private readonly List<SingleBasicData> singleDataList;

        public BasicConnectorModal(string from, string to)
        {
            InitializeComponent();
            singleDataList = new List<SingleBasicData>();
            InstructionsBlock.Text = string.Format("Connect Between {0} to {1}:", from, to);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ConnectorData = new BasicAutomatonData(singleDataList);
            this.DialogResult = true;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddBtn_Click(null, null);
            else if (e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InputBox.Focus();
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.InputBox.Text.Length == 0) return;

            var singleData = new SingleBasicData(InputBox.Text.Single());
            InsertNewEntryToStack(singleData);
            this.InputBox.Clear();
        }

        private void InsertNewEntryToStack(SingleBasicData newEntry)
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
            var taggedEntry = taggedBorder!.Tag as SingleBasicData;

            this.InsertedConnectionsPanel.Children.Remove(taggedBorder);
            this.singleDataList.Remove(taggedEntry!);
        }
    }
}

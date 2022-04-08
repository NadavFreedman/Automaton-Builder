﻿using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using Newtonsoft.Json;
using Petzold.Media2D;
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

namespace AutomatonBuilder.Entities
{
    /// <summary>
    /// Interaction logic for ModelNode.xaml
    /// </summary>
    public partial class ModelNode : UserControl, IMoveable
    {
        public int Size { get; set; }

        private bool accepting;

        public bool Starting;

        [JsonIgnore]
        public ArrowLine startingArrow;

        [JsonIgnore]
        private readonly MainWindow root;

        private Point position;

        public List<IConnector> connectedLinesToThisNode;
        public List<IConnector> connectedLinesFromThisNode;


        private int index;
        public int Index { get { return index; } set { this.index = value; this.IndexText.Text = this.index.ToString(); } }

        public ModelNode(int index, MainWindow host, Point pos)
        {
            InitializeComponent();
            this.Index = index;
            this.root = host;
            this.accepting = false;
            this.Resize(70);
            this.position = new Point();
            this.Starting = false;
            this.position = pos;

            this.startingArrow = new ArrowLine()
            {
                Visibility = Visibility.Visible,
                StrokeThickness = 3,
                Stroke = Brushes.Black,

                X1 = pos.X - 45,
                Y1 = pos.Y + 45,

                X2 = pos.X - 25,
                Y2 = pos.Y + 25
            };

            this.connectedLinesToThisNode = new List<IConnector>();
            this.connectedLinesFromThisNode = new List<IConnector>();
        }

        public void Resize(int size)
        {
            MainViewBox.Width = size;
            MainViewBox.Height = size;
            this.Size = size;
        }

        private void RemoveNodeClick(object sender, RoutedEventArgs e)
        {
            root.RemoveNode(this);
        }

        public override string ToString()
        {
            return string.Format("q{0}", this.index);
        }

        public void SetPosition(Point newPosition)
        {
            this.position = newPosition;
            this.startingArrow.X1 = newPosition.X - 45;
            this.startingArrow.Y1 = newPosition.Y + 45;
            this.startingArrow.X2 = newPosition.X - 25;
            this.startingArrow.Y2 = newPosition.Y + 25;

            Canvas.SetLeft(this, this.position.X - this.Size / 2);
            Canvas.SetTop(this, this.position.Y - this.Size / 2);

            foreach(IConnector connector in this.connectedLinesToThisNode)
            {
                connector.SetConnectorEnd(newPosition);
            }
            foreach (IConnector connector in this.connectedLinesFromThisNode)
            {
                connector.SetConnectorStart(newPosition);
            }
        }

        public Point GetPosition()
        {
            return this.position;
        }

        private void AcceptingClick(object sender, RoutedEventArgs e)
        {
            if (this.accepting)
            {
                this.AcceptingMenuItem.Header = "Accepting";
                this.AcceptingElipse.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.AcceptingMenuItem.Header = "Not accepting";
                this.AcceptingElipse.Visibility = Visibility.Visible;
            }
            this.accepting = !this.accepting;
        }

        private void StartingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NodeUtils.ToggleNodeStarting(this.root.context, this);
        }
    }
}

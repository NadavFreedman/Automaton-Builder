using AutomatonBuilder.Interfaces;
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
        private int size;

        public bool Accepting { get; set; }

        public bool Starting { get; set; }

        [JsonIgnore]
        public ArrowLine StartingArrow { get; set; }

        [JsonIgnore]
        private readonly MainEditingScreen root;

        private Point position;
        public Dictionary<ModelNode, IConnector> ConnectorsToThisNode { get; set; }
        public Dictionary<ModelNode, IConnector> ConnectorsFromThisNode { get; set; }


        private int index;
        public int Index { get { return index; } set { this.index = value; this.IndexText.Text = this.index.ToString(); } }

        public ModelNode(int index, MainEditingScreen host, Point pos)
        {
            InitializeComponent();
            this.Index = index;
            this.root = host;
            this.Accepting = false;
            this.Resize(70);
            this.Starting = false;

            this.StartingArrow = new ArrowLine()
            {
                Visibility = Visibility.Visible,
                StrokeThickness = 3,
                Stroke = Brushes.Black,
            };

            this.ConnectorsToThisNode = new Dictionary<ModelNode, IConnector>();
            this.ConnectorsFromThisNode = new Dictionary<ModelNode, IConnector>();

            this.SetPosition(pos);
        }

        public void Resize(int size)
        {
            MainViewBox.Width = size;
            MainViewBox.Height = size;
            this.size = size;
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
            this.StartingArrow.X1 = newPosition.X - 45;
            this.StartingArrow.Y1 = newPosition.Y + 45;
            this.StartingArrow.X2 = newPosition.X - 25;
            this.StartingArrow.Y2 = newPosition.Y + 25;

            Canvas.SetLeft(this, this.position.X - this.size / 2);
            Canvas.SetTop(this, this.position.Y - this.size / 2);

            foreach(IConnector connector in this.ConnectorsToThisNode.Values)
            {
                connector.SetConnectorEnd(newPosition);
            }
            foreach (IConnector connector in this.ConnectorsFromThisNode.Values)
            {
                connector.SetConnectorStart(newPosition);
            }
        }

        public Point GetPosition()
        {
            return this.position;
        }

        public void AcceptingClick(object sender, RoutedEventArgs e)
        {
            if (this.Accepting)
            {
                this.AcceptingMenuItem.Header = "Accepting";
                this.AcceptingElipse.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.AcceptingMenuItem.Header = "Not accepting";
                this.AcceptingElipse.Visibility = Visibility.Visible;
            }
            this.Accepting = !this.Accepting;
        }

        private void StartingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NodeUtils.ToggleNodeStarting(this.root.context, this);
        }

        public void SetColor(Brush color)
        {
            this.NodeBody.Fill = color;
        }

    }
}

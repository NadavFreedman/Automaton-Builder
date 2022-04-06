using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutomatonBuilder.Entities.Connectors
{
    public class SelfConnector : IConnector
    {
        public string ConnectorData { get; private set; }

        private Ellipse ellipse;
        private Point nodeCenter;
        private Border borderedText;
        private FormattedText formmattedText;

        public SelfConnector(string text, Point nodeCenter)
        {
            InitBorder(text);
            ChangeConnectorData(text);
            this.nodeCenter = nodeCenter;
            this.ellipse = new Ellipse
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                Visibility = Visibility.Visible
            };
        }

        private void InitBorder(string text)
        {
            this.borderedText = TextUtils.CreateBorderWithTextBlock(text);
        }

        public void ChangeConnectorData(string text)
        {
            this.ConnectorData = text;
            TextBlock childText = (TextBlock)this.borderedText.Child;
            this.formmattedText = TextUtils.CreateFormattedText(childText);
            childText.Text = text;
        }

        public void AddToCanvasButtom(Canvas canvas)
        {
            canvas.Children.Insert(0, this.borderedText);
            canvas.Children.Insert(0, this.ellipse);
        }

        public void SetTextPosition()
        {
            Point textCoords = new(this.nodeCenter.X - 80 / 2, this.nodeCenter.Y - 80 / 2);
            TextUtils.SetPositionForText(this.borderedText, textCoords, this.formmattedText);
        }

        public void SetConnectorPosition()
        {
            Canvas.SetLeft(this.ellipse, this.nodeCenter.X - 45);
            Canvas.SetTop(this.ellipse, this.nodeCenter.Y - 45);
        }


        public void BindConnectorToMainWindow(MainWindow mainWindow)
        {
            ConnectorUtils.AddContextMenuToConnectorElement(this.ellipse, mainWindow, this);
            ConnectorUtils.AddContextMenuToConnectorElement(this.borderedText, mainWindow, this);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(this.ellipse);
            canvas.Children.Remove(this.borderedText);
        }

        public void SetConnectorStart(Point startingPoint)
        {
            this.nodeCenter = startingPoint;
            this.SetConnectorPosition();
            this.SetTextPosition();
        }

        public void SetConnectorEnd(Point endPoint)
        {
            this.nodeCenter = endPoint;
            this.SetConnectorPosition();
            this.SetTextPosition();
        }
    }
}

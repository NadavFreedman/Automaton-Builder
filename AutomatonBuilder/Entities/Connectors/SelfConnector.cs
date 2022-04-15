using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.TextElements;
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
        private BorderedText text;

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
            this.text = new(text);
        }

        public void ChangeConnectorData(string text)
        {
            this.ConnectorData = text;
            this.text.SetText(text);
        }

        public void AddToCanvasButtom(Canvas canvas)
        {
            this.text.AddToCanvas(canvas, ZAxis.Bottom);
            canvas.Children.Insert(0, this.ellipse);
        }

        public void SetTextPosition()
        {
            Point textCoords = new(this.nodeCenter.X - 80 / 2, this.nodeCenter.Y - 80 / 2);
            this.text.SetPosition(textCoords);
        }

        public void SetConnectorPosition()
        {
            Canvas.SetLeft(this.ellipse, this.nodeCenter.X - 45);
            Canvas.SetTop(this.ellipse, this.nodeCenter.Y - 45);
        }


        public void BindConnectorToMainWindow(MainEditingScreen mainWindow)
        {
            ConnectorUtils.AddContextMenuToConnectorLine(this.ellipse, mainWindow, this);
            ConnectorUtils.AddContextMenuToConnectorText(this.text, mainWindow, this);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(this.ellipse);
            this.text.RemoveFromCanvas(canvas);
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

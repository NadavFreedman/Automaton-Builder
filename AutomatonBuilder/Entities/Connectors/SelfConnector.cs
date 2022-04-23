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
    public class SelfConnector : IConnector, IMoveable
    {
        public IConnectorData ConnectorData { get; private set; }

        private Ellipse ellipse;
        private Point nodeCenter;
        private Point connectorCenter;
        private BorderedText borderedText;

        public SelfConnector(IConnectorData data, Point nodeCenter)
        {
            InitBorder(data.ToString());
            ChangeConnectorData(data);
            this.nodeCenter = nodeCenter;
            this.ellipse = new Ellipse
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                Visibility = Visibility.Visible
            };
            var initialPoint = new Point
            {
                X = nodeCenter.X - 100,
                Y = nodeCenter.Y - 100
            };
            this.SetPosition(initialPoint);
        }

        private void InitBorder(string text)
        {
            this.borderedText = new(text);
        }

        public void ChangeConnectorData(IConnectorData data)
        {
            this.ConnectorData = data;
            this.borderedText.SetText(data.ToString());
        }

        public void AddToCanvasButtom(Canvas canvas)
        {
            this.borderedText.AddToCanvas(canvas, ZAxis.Bottom);
            canvas.Children.Insert(0, this.ellipse);
        }


        public void BindConnectorToMainWindow(MainEditingScreen mainWindow)
        {
            this.ellipse.Tag = this;
            this.borderedText.Border.Tag = this;

            this.ellipse.MouseEnter += mainWindow.TaggedElement_MouseEnter;
            this.borderedText.Border.MouseEnter += mainWindow.TaggedElement_MouseEnter;
            this.borderedText.Border.MouseLeave += mainWindow.Element_MouseLeave;
            this.ellipse.MouseLeave += mainWindow.Element_MouseLeave;


            ConnectorUtils.AddContextMenuToConnectorLine(this.ellipse, mainWindow, this);
            ConnectorUtils.AddContextMenuToConnectorText(this.borderedText, mainWindow, this);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(this.ellipse);
            this.borderedText.RemoveFromCanvas(canvas);
        }

        public void SetConnectorStart(Point startingPoint)
        {
            double deltaX = this.nodeCenter.X - startingPoint.X;
            double deltaY = this.nodeCenter.Y - startingPoint.Y;
            this.nodeCenter = startingPoint;

            this.connectorCenter.X += deltaX;
            this.connectorCenter.Y += deltaY;
            this.SetPosition(this.connectorCenter);
        }

        public void SetConnectorEnd(Point endPoint)
        {
            double deltaX = this.nodeCenter.X - endPoint.X;
            double deltaY = this.nodeCenter.Y - endPoint.Y;
            this.nodeCenter = endPoint;

            this.connectorCenter.X += deltaX;
            this.connectorCenter.Y += deltaY;
            this.SetPosition(this.connectorCenter);
        }

        public void SetPosition(Point newPosition)
        {
            const double NODE_SIZE = 70;
            const double SMALL_CIRCLE_SIZE = 50;

            var alpha = ConnectorUtils.GetAlpha(this.nodeCenter, newPosition);
            var newPosOnCircle = new Point
            {
                X = this.nodeCenter.X - Math.Cos(alpha) * NODE_SIZE * 0.5 - Math.Cos(alpha) * SMALL_CIRCLE_SIZE * 0.1,
                Y = this.nodeCenter.Y - Math.Sin(alpha) * NODE_SIZE * 0.5 - Math.Sin(alpha) * SMALL_CIRCLE_SIZE * 0.1
            };
            var newTextPos = new Point
            {
                X = newPosOnCircle.X - Math.Cos(alpha) * SMALL_CIRCLE_SIZE * 0.5 - Math.Cos(alpha) * this.borderedText.Width * 0.5,
                Y = newPosOnCircle.Y - Math.Sin(alpha) * SMALL_CIRCLE_SIZE * 0.5 - Math.Sin(alpha) * this.borderedText.Height * 0.5
            };
            this.connectorCenter = newPosOnCircle;
            Canvas.SetLeft(this.ellipse, newPosOnCircle.X - SMALL_CIRCLE_SIZE / 2);
            Canvas.SetTop(this.ellipse, newPosOnCircle.Y - SMALL_CIRCLE_SIZE / 2);

            this.borderedText.SetPosition(newTextPos);
        }

        public Point GetPosition()
        {
            return this.connectorCenter;
        }
    }
}

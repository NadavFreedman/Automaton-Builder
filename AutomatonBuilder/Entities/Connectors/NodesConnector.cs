using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using Petzold.Media2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.Connectors
{
    public class NodesConnector: IConnector
    {
        public string ConnectorData { get; private set; }
        public Point ConnectorMiddlePoint 
        { 
            get { return this.arrow.Points[1]; } 
            set 
            {
                this.arrow.Points[1] = value;
                this.SetTextPosition();
            } 
        }

        const int NODE_SIZE = 80;

        private FormattedText? formmattedText;
        private Border borderedText;
        private ArrowPolyline arrow;
        private Point startingPoint;
        private Point endPoint;

        public NodesConnector(string text, Point startingPoint, Point endPoint)
        {
            InitBorder(text);
            ChangeConnectorData(text);
            InitArrow(startingPoint, endPoint);
        }

        private void InitBorder(string text)
        {
            this.borderedText = TextUtils.CreateBorderWithTextBlock(text);
        }

        private void InitArrow(Point startingPoint, Point endPoint)
        {
            this.startingPoint = startingPoint;
            this.endPoint = endPoint;
            this.arrow = new ArrowPolyline
            {
                Visibility = Visibility.Visible,
                StrokeThickness = 3,
                Stroke = Brushes.Black,
                Points = InitArrowPoints(startingPoint, endPoint)
            };
        }

        private static PointCollection InitArrowPoints(Point startingPoint, Point endPoint)
        {
            //Calculate the positions of the arrow.
            double deltaX = endPoint.X - startingPoint.X;
            double deltaY = endPoint.Y - startingPoint.Y;
            double alpha = Math.Atan(deltaY / deltaX);
            if (deltaX < 0 && deltaY < 0 || deltaY > 0 && deltaX < 0)
                alpha -= Math.PI;

            return new PointCollection
            {
                new Point
                {
                    X = startingPoint.X,
                    Y = startingPoint.Y
                },
                new Point
                {
                    X = (startingPoint.X + endPoint.X - Math.Cos(alpha) * (NODE_SIZE / 2)) / 2,
                    Y = (startingPoint.Y + endPoint.Y - Math.Sin(alpha) * (NODE_SIZE / 2)) / 2
                },
                new Point
                {
                    X = endPoint.X - Math.Cos(alpha) * (NODE_SIZE / 2),
                    Y = endPoint.Y - Math.Sin(alpha) * (NODE_SIZE / 2)
                }
            };
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
            canvas.Children.Insert(0, this.arrow);
        }

        public void BindConnectorToMainWindow(MainWindow mainWindow)
        {
            this.arrow.Tag = this;
            this.borderedText.Tag = this;

            this.arrow.MouseEnter += mainWindow.Connector_MouseEnter;
            this.borderedText.MouseEnter += mainWindow.Element_MouseLeave;
            this.borderedText.MouseLeave += mainWindow.Element_MouseLeave;
            this.arrow.MouseLeave += mainWindow.Connector_MouseEnter;

            ConnectorUtils.AddContextMenuToConnectorElement(this.arrow, mainWindow, this);
            ConnectorUtils.AddContextMenuToConnectorElement(this.borderedText, mainWindow, this);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(this.arrow);
            canvas.Children.Remove(this.borderedText);
        }

        public void SetTextPosition()
        {
            int pointCount = this.arrow.Points.Count;
            Point middlePoint;
            if (pointCount % 2 != 0)
            {
                middlePoint = this.arrow.Points[pointCount / 2];
            }
            else
            {
                middlePoint = new Point()
                {
                    X = this.arrow.Points.Sum(p => p.X) / pointCount,
                    Y = this.arrow.Points.Sum(p => p.Y) / pointCount
                };

            }
            TextUtils.SetPositionForText(this.borderedText, middlePoint, this.formmattedText);
        }

        public void SetConnectorStart(Point newStartingPoint)
        {
            this.startingPoint = newStartingPoint;
            this.arrow.Points = InitArrowPoints(newStartingPoint, this.endPoint);
            this.SetTextPosition();
        }

        public void SetConnectorEnd(Point newEndPoint)
        {
            this.endPoint = newEndPoint;
            this.arrow.Points = InitArrowPoints(this.startingPoint, newEndPoint);
            this.SetTextPosition();
        }
    }
}

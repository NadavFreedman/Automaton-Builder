using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using Petzold.Media2D;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.Connectors
{
    public class NodesConnector: IConnector, IMoveable
    {
        public string ConnectorData { get; private set; }
        public Point ConnectorMiddlePoint 
        { 
            get { return this.arrow.Points[1]; } 
        }

        const int NODE_SIZE = 80;

        private BorderedText text;
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
            this.text = new(text);
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
            Point middlePoint = new Point
            {
                X = (startingPoint.X + endPoint.X) / 2,
                Y = (startingPoint.Y + endPoint.Y) / 2
            };

            double alpha = ConnectorUtils.GetAlpha(endPoint, middlePoint);

            return new PointCollection
            {
                new Point
                {
                    X = startingPoint.X,
                    Y = startingPoint.Y
                },
                middlePoint,
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
            this.text.SetText(text);
        }

        public void AddToCanvasButtom(Canvas canvas)
        {
            this.text.AddToCanvas(canvas, ZAxis.Bottom);
            canvas.Children.Insert(0, this.arrow);
        }

        public void BindConnectorToMainWindow(MainWindow mainWindow)
        {
            this.arrow.Tag = this;
            this.text.Border.Tag = this;

            this.arrow.MouseEnter += mainWindow.TaggedElement_MouseEnter;
            this.text.Border.MouseEnter += mainWindow.TaggedElement_MouseEnter;
            this.text.Border.MouseLeave += mainWindow.Element_MouseLeave;
            this.arrow.MouseLeave += mainWindow.Element_MouseLeave;

            ConnectorUtils.AddContextMenuToConnectorLine(this.arrow, mainWindow, this);
            ConnectorUtils.AddContextMenuToConnectorText(this.text, mainWindow, this);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(this.arrow);
            this.text.RemoveFromCanvas(canvas);
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
            this.text.SetPosition(middlePoint);
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

        public void SetPosition(Point newPosition)
        {
            this.arrow.Points[1] = newPosition;
            double alpha = ConnectorUtils.GetAlpha(this.endPoint, newPosition);
            this.arrow.Points[2] = new Point
            {
                X = endPoint.X - Math.Cos(alpha) * (NODE_SIZE / 2),
                Y = endPoint.Y - Math.Sin(alpha) * (NODE_SIZE / 2)
            };
            this.SetTextPosition();
        }
    }
}

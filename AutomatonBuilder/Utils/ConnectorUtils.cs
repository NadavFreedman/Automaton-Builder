using AutomatonBuilder.Entities;
using Petzold.Media2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutomatonBuilder.Utils
{
    public class ConnectorUtils
    {
        public static UIElement ConnectNodes(AutomatonContext context, ModelNode source, ModelNode destination, string connector, MainWindow host)
        {

            Border box = TextUtils.CreateBorderWithTextBlock(connector);
            FormattedText formattedText = TextUtils.CreateFormattedText((TextBlock)box.Child);
            if (source == destination)
            {
                return ConnectNodeToSelf(context, source, box, formattedText, host);
            }
            return ConnectNodeToAnotherNode(context, source, destination, box, formattedText, host);
        }

        public static List<ModelNode> DisconnectConnector(AutomatonContext context, UIElement connector)
        {
            List<ModelNode> disconnectedNodes = new List<ModelNode>();

            if (((FrameworkElement)connector).Tag is not Border)
                connector = (UIElement)((FrameworkElement)connector).Tag;

            foreach (var node in context.NodesList)
            {
                if (node.connectedLines.Contains(connector))
                {
                    node.connectedLines.Remove(connector);
                    disconnectedNodes.Add(node);
                }
            }
            RemoveConnector(context.MainCanvas, connector);
            return disconnectedNodes;
        }
        private static void RemoveConnector(Canvas mainCanvas, UIElement connector)
        {
            mainCanvas.Children.Remove(connector);
            if (((FrameworkElement)connector).Tag != null)
                mainCanvas.Children.Remove((UIElement)((FrameworkElement)connector).Tag);
        }

        private static UIElement ConnectNodeToAnotherNode(AutomatonContext context, ModelNode connectFrom, ModelNode connectTo, Border border, FormattedText formattedText, MainWindow host)
        {
            //Calculate the positions of the arrow.
            double deltaX = connectTo.Position.X - connectFrom.Position.X;
            double deltaY = connectTo.Position.Y - connectFrom.Position.Y;
            double alpha = Math.Atan(deltaY / deltaX);
            if (deltaX < 0 && deltaY < 0 || deltaY > 0 && deltaX < 0)
                alpha -= Math.PI;
            ArrowLine connector = new ArrowLine
            {
                Visibility = Visibility.Visible,
                StrokeThickness = 3,
                Stroke = Brushes.Black,

                X1 = connectFrom.Position.X,
                Y1 = connectFrom.Position.Y,

                X2 = connectTo.Position.X - Math.Cos(alpha) * (connectTo.Size / 2),
                Y2 = connectTo.Position.Y - Math.Sin(alpha) * (connectTo.Size / 2)
            };

            //Add a context menu to the arrow
            AddRemoveMenuToConnectorElement(connector, host);

            //Add a context menu to the text block
            AddRemoveMenuToConnectorElement(border, host);
            connector.Tag = border;
            border.Tag = connector;

            //calculate the middle point of the line
            Point textCoords = new(connectFrom.Position.X + deltaX / 2, connectFrom.Position.Y + deltaY / 2);
            SetPositionForText(border, formattedText, textCoords);

            //Add the text to the canvas
            ReconnectConnector(context, connector, connectFrom, connectTo);

            return connector;
        }

        private static UIElement ConnectNodeToSelf(AutomatonContext context, ModelNode node, Border border, FormattedText formattedText, MainWindow host)
        {
            Ellipse connectorEllipse = new Ellipse
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                Visibility = Visibility.Visible
            };

            AddRemoveMenuToConnectorElement(connectorEllipse, host);

            AddRemoveMenuToConnectorElement(border, host);

            connectorEllipse.Tag = border;
            border.Tag = connectorEllipse;

            SetPositionForEllipseConnector(connectorEllipse, node);
            Point textCoords = new(node.Position.X - node.Size / 2, node.Position.Y - node.Size / 2);
            SetPositionForText(border, formattedText, textCoords);
            ReconnectConnector(context, connectorEllipse, node);

            return connectorEllipse;
        }

        private static void AddRemoveMenuToConnectorElement(FrameworkElement connectorElement, MainWindow host)
        {
            MenuItem removeConnector = new MenuItem
            {
                Header = "Remove",
                Tag = connectorElement
            };
            removeConnector.Click += host.RemoveConnector_Click;
            connectorElement.ContextMenu = new ContextMenu();
            connectorElement.ContextMenu.Items.Add(removeConnector);
        }

        private static void AddTextToCanvas(Canvas mainCanvas, UIElement text)
        {
            mainCanvas.Children.Add(text);
        }

        public static void AddConnectorAndTextToCanvas(Canvas mainCanvas, UIElement connector)
        {
            Border taggedElement = ((FrameworkElement)connector).Tag as Border;
            mainCanvas.Children.Insert(0, connector);
            AddTextToCanvas(mainCanvas, taggedElement);
        }

        private static void SetPositionForText(Border text, FormattedText formattedText, Point textCoords)
        {
            Canvas.SetLeft(text, textCoords.X - 2 - formattedText.Width / 2);
            Canvas.SetTop(text, textCoords.Y - 2 - formattedText.Height / 2);
        }

        private static void SetPositionForEllipseConnector(Ellipse connector, ModelNode node)
        {
            Canvas.SetLeft(connector, node.Position.X - 45);
            Canvas.SetTop(connector, node.Position.Y - 45);
        }

        public static void ReconnectConnector(AutomatonContext context, UIElement connector, ModelNode source, ModelNode? destination = null)
        {
            UIElement taggedElement = ((FrameworkElement)connector).Tag as UIElement;

            UIElement line = taggedElement is not Border ? taggedElement : connector;

            if (destination == null)
            {
                source.connectedLines.Add(line);
            }
            else
            {
                source.connectedLines.Add(line);
                destination.connectedLines.Add(line);
            }
            AddConnectorAndTextToCanvas(context.MainCanvas, line);
        }

    }
}

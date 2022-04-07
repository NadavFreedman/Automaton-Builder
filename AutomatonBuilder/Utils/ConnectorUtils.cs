using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Connectors;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Utils
{
    public class ConnectorUtils
    {
        public static IConnector ConnectNodes(AutomatonContext context, ModelNode source, ModelNode destination, string connectorData, MainWindow host)
        {
            if (source == destination)
                return ConnectNodeToSelf(context, source, connectorData, host);
            return ConnectNodeToAnotherNode(context, source, destination, connectorData, host);
        }

        public static List<ModelNode> DisconnectConnector(AutomatonContext context, IConnector connector)
        {
            List<ModelNode> disconnectedNodes = new List<ModelNode>();

            foreach (var node in context.NodesList)
            {
                if (node.connectedLinesFromThisNode.Contains(connector) || node.connectedLinesToThisNode.Contains(connector))
                {
                    node.connectedLinesFromThisNode.Remove(connector);
                    disconnectedNodes.Add(node);
                }
            }
            connector.RemoveFromCanvas(context.MainCanvas);
            return disconnectedNodes;
        }

        private static IConnector ConnectNodeToAnotherNode(AutomatonContext context, ModelNode source, ModelNode destination, string connectorData, MainWindow host)
        {
            

            NodesConnector connector = new NodesConnector(connectorData, source.Position, destination.Position);

            connector.SetTextPosition();
            connector.AddToCanvasButtom(context.MainCanvas);
            connector.BindConnectorToMainWindow(host);
            source.connectedLinesFromThisNode.Add(connector);
            destination.connectedLinesToThisNode.Add(connector);

            return connector;
        }

        private static IConnector ConnectNodeToSelf(AutomatonContext context, ModelNode node, string connectorData, MainWindow host)
        {
            SelfConnector connector = new SelfConnector(connectorData, node.Position);
            connector.SetTextPosition();
            connector.SetConnectorPosition();
            connector.AddToCanvasButtom(context.MainCanvas);
            connector.BindConnectorToMainWindow(host);

            node.connectedLinesFromThisNode.Add(connector);

            return connector;
        }

        public static void AddContextMenuToConnectorLine(FrameworkElement connectorElement, MainWindow host, IConnector connector)
        {
            MenuItem removeConnector = new MenuItem
            {
                Header = "Remove",
                Tag = connector
            };
            removeConnector.Click += host.RemoveConnector_Click;
            connectorElement.ContextMenu = new ContextMenu();
            connectorElement.ContextMenu.Items.Add(removeConnector);
        }

        public static void AddContextMenuToConnectorText(BorderedText borderedText, MainWindow host, IConnector connector)
        {
            MenuItem removeConnector = new MenuItem
            {
                Header = "Remove",
                Tag = connector
            };
            removeConnector.Click += host.RemoveConnector_Click;
            var menu = new ContextMenu();
            menu.Items.Add(removeConnector);
            borderedText.AttachContextMenu(menu);
        }


        public static void ReconnectConnector(AutomatonContext context, IConnector connector, ModelNode source, ModelNode? destination = null)
        {
            source.connectedLinesFromThisNode.Add(connector);
            destination?.connectedLinesToThisNode.Add(connector);
            connector.AddToCanvasButtom(context.MainCanvas);
        }

        public static ModelNode? GetNodeByName(AutomatonContext context, string nodeName)
        {
            foreach (ModelNode node in context.NodesList)
            {
                if (node.ToString() == nodeName)
                    return node;
            }
            return null;   
        }

        public static double GetAlpha(Point a, Point b)
        {
            double deltaX = a.X - b.X;
            double deltaY = a.Y - b.Y;
            double alpha = Math.Atan(deltaY / deltaX);
            if (deltaX < 0 && deltaY < 0 || deltaY > 0 && deltaX < 0)
                return alpha - Math.PI;
            return alpha;
        }

    }
}

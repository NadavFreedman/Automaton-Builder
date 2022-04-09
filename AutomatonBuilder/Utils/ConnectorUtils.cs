using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Connectors;
using AutomatonBuilder.Entities.Contexts;
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
                if (node.ConnectedLinesFromThisNode.ContainsKey(connector) || node.ConnectedLinesToThisNode.ContainsKey(connector))
                {
                    node.ConnectedLinesFromThisNode.Remove(connector);
                    disconnectedNodes.Add(node);
                }
            }
            connector.RemoveFromCanvas(context.MainCanvas);
            return disconnectedNodes;
        }

        private static IConnector ConnectNodeToAnotherNode(AutomatonContext context, ModelNode source, ModelNode destination, string connectorData, MainWindow host)
        {
            NodesConnector connector = new NodesConnector(connectorData, source.GetPosition(), destination.GetPosition());

            connector.SetTextPosition();
            connector.AddToCanvasButtom(context.MainCanvas);

            connector.BindConnectorToMainWindow(host);
            source.ConnectedLinesFromThisNode[connector] = destination;
            destination.ConnectedLinesToThisNode[connector] = source;

            return connector;
        }

        private static IConnector ConnectNodeToSelf(AutomatonContext context, ModelNode node, string connectorData, MainWindow host)
        {
            SelfConnector connector = new SelfConnector(connectorData, node.GetPosition());
            connector.SetTextPosition();
            connector.SetConnectorPosition();
            connector.AddToCanvasButtom(context.MainCanvas);
            connector.BindConnectorToMainWindow(host);

            node.ConnectedLinesFromThisNode[connector] = node;

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
            source.ConnectedLinesFromThisNode[connector] = destination ?? source;
            destination?.ConnectedLinesToThisNode.Add(connector, source);
            connector.AddToCanvasButtom(context.MainCanvas);
        }

        public static ModelNode GetNodeByName(AutomatonContext context, string nodeName)
        {
            foreach (ModelNode node in context.NodesList)
            {
                if (node.ToString() == nodeName)
                    return node;
            }
            throw new Exception($"No node with the name {nodeName}");
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

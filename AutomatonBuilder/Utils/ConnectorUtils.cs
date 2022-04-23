using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Connectors;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Modals.ConnectionModals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Utils
{
    public class ConnectorUtils
    {
        public static IConnector ConnectNodes(AutomatonContext context, ModelNode source, ModelNode destination, IConnectorData connectorData, MainEditingScreen host)
        {
            if (source == destination)
                return ConnectNodeToSelf(context.MainCanvas, source, connectorData, host);
            return ConnectNodeToAnotherNode(context.MainCanvas, source, destination, connectorData, host);
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

        public static NodesConnector ConnectNodeToAnotherNode(Canvas canvas, ModelNode source, ModelNode destination, IConnectorData connectorData, MainEditingScreen? host = null)
        {
            NodesConnector connector = new NodesConnector(connectorData, source.GetPosition(), destination.GetPosition());

            connector.SetTextPosition();
            connector.AddToCanvasButtom(canvas);

            if (host is not null)
                connector.BindConnectorToMainWindow(host);

            source.ConnectedLinesFromThisNode[connector] = destination;
            destination.ConnectedLinesToThisNode[connector] = source;

            return connector;
        }

        public static SelfConnector ConnectNodeToSelf(Canvas canvas, ModelNode node, IConnectorData connectorData, MainEditingScreen? host = null)
        {
            SelfConnector connector = new SelfConnector(connectorData, node.GetPosition());
            connector.AddToCanvasButtom(canvas);

            if (host is not null)
                connector.BindConnectorToMainWindow(host);

            node.ConnectedLinesFromThisNode[connector] = node;

            return connector;
        }

        public static void AddContextMenuToConnectorLine(FrameworkElement connectorElement, MainEditingScreen host, IConnector connector)
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

        public static void AddContextMenuToConnectorText(BorderedText borderedText, MainEditingScreen host, IConnector connector)
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
            if (deltaX < 0 && deltaY <= 0 || deltaY > 0 && deltaX < 0)
                return alpha - Math.PI;
            return alpha;
        }

        public static Point CalculateArrowEndpoint(Point endPoint, double alpha, double node_size)
        {
            return new Point
            {
                X = endPoint.X - Math.Cos(alpha) * node_size * 0.55,
                Y = endPoint.Y - Math.Sin(alpha) * node_size * 0.55
            };
        }

        public static IConnectionModal CreateConnectionModal(AutomatonTypes type, string from, string to)
        {
            switch (type)
            {
                case AutomatonTypes.Basic:
                    return new BasicConnectorModal(from, to);

                case AutomatonTypes.Pushdown:
                    return new PushdownConnectorModal(from, to);

                case AutomatonTypes.Turing:
                    return new BasicConnectorModal(from, to);

                default:
                    throw new Exception("Unknown automaton type");
            }
        }

    }
}

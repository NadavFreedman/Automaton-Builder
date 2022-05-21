using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Connectors;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.Exceptions;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Modals.ConnectionModals;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<ModelNode> disconnectedNodes = new();

            foreach (var node in context.NodesList)
            {
                if (node.ConnectorsFromThisNode.ContainsValue(connector))
                {
                    ModelNode otherNode = node.ConnectorsFromThisNode.First(x => x.Value == connector).Key;
                    node.ConnectorsFromThisNode.Remove(otherNode);
                    disconnectedNodes.Add(node);
                }
                else if (node.ConnectorsToThisNode.ContainsValue(connector))
                {
                    ModelNode otherNode = node.ConnectorsToThisNode.First(x => x.Value == connector).Key;
                    node.ConnectorsToThisNode.Remove(otherNode);
                    disconnectedNodes.Add(node);
                }
            }
            connector.RemoveFromCanvas(context.MainCanvas);
            return disconnectedNodes;
        }

        public static NodesConnector ConnectNodeToAnotherNode(Canvas canvas, ModelNode source, ModelNode destination, IConnectorData connectorData, MainEditingScreen? host = null)
        {
            NodesConnector connector = new(connectorData, source.GetPosition(), destination.GetPosition());

            connector.SetTextPosition();
            connector.AddToCanvasButtom(canvas);

            if (host is not null)
                connector.BindConnectorToMainWindow(host);

            source.ConnectorsFromThisNode[destination] = connector;
            destination.ConnectorsToThisNode[source] = connector;

            return connector;
        }

        public static SelfConnector ConnectNodeToSelf(Canvas canvas, ModelNode node, IConnectorData connectorData, MainEditingScreen? host = null)
        {
            SelfConnector connector = new SelfConnector(connectorData, node.GetPosition());
            connector.AddToCanvasButtom(canvas);

            if (host is not null)
                connector.BindConnectorToMainWindow(host);

            node.ConnectorsFromThisNode[node] = connector;

            return connector;
        }

        public static void AddContextMenuToConnectorLine(FrameworkElement connectorElement, MainEditingScreen host, IConnector connector)
        {
            MenuItem removeConnector = new MenuItem
            {
                Header = "Remove",
                Tag = connector
            };
            MenuItem editConnector = new MenuItem
            {
                Header = "Edit...",
                Tag = connector
            };
            removeConnector.Click += host.RemoveConnector_Click;
            editConnector.Click += host.EditConnector_Click;
            connectorElement.ContextMenu = new ContextMenu();
            connectorElement.ContextMenu.Items.Add(removeConnector);
            connectorElement.ContextMenu.Items.Add(editConnector);
        }



        public static void AddContextMenuToConnectorText(BorderedText borderedText, MainEditingScreen host, IConnector connector)
        {
            MenuItem removeConnector = new()
            {
                Header = "Remove",
                Tag = connector
            };
            MenuItem editConnector = new()
            {
                Header = "Edit...",
                Tag = connector
            };
            removeConnector.Click += host.RemoveConnector_Click;
            editConnector.Click += host.EditConnector_Click;
            var menu = new ContextMenu();
            menu.Items.Add(removeConnector);
            menu.Items.Add(editConnector);
            borderedText.AttachContextMenu(menu);
        }


        public static void ReconnectConnector(AutomatonContext context, IConnector connector, ModelNode source, ModelNode? destination = null)
        {
            if (destination is null)
                destination = source;
            source.ConnectorsFromThisNode[destination] = connector;
            destination.ConnectorsToThisNode[source] = connector;
            connector.AddToCanvasButtom(context.MainCanvas);
        }

        public static ModelNode GetNodeByName(AutomatonContext context, string nodeName)
        {
            foreach (ModelNode node in context.NodesList)
            {
                if (node.ToString() == nodeName)
                    return node;
            }
            throw new BuilderNotFoundException($"No node with the name {nodeName}");
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
                    return new TuringConnectorModal(from, to);

                default:
                    throw new BuilderUnsupportedTypeException("Unknown automaton type");
            }
        }

        public static IConnectionModal CreateConnectionModal(AutomatonTypes type, string from, string to, IConnectorData currentData)
        {
            switch (type)
            {
                case AutomatonTypes.Basic:
                    return new BasicConnectorModal(from, to, currentData);

                case AutomatonTypes.Pushdown:
                    return new PushdownConnectorModal(from, to, currentData);

                case AutomatonTypes.Turing:
                    return new TuringConnectorModal(from, to, currentData);

                default:
                    throw new BuilderUnsupportedTypeException("Unknown automaton type");
            }
        }
    }
}

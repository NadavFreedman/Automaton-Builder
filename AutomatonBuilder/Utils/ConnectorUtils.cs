using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Connectors;
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

        public static void AddContextMenuToConnectorElement(FrameworkElement connectorElement, MainWindow host, IConnector connector)
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


        public static void ReconnectConnector(AutomatonContext context, IConnector connector, ModelNode source, ModelNode? destination = null)
        {
            source.connectedLinesFromThisNode.Add(connector);
            destination?.connectedLinesToThisNode.Add(connector);
            connector.AddToCanvasButtom(context.MainCanvas);
        }

    }
}

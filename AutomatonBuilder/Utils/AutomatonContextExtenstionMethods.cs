using AutomatonBuilder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AutomatonBuilder.Utils
{
    public static class AutomatonContextExtenstionMethods
    {
        #region StartingNodeActions
        private static void SetStartingNodeToNull(AutomatonContext context)
        {
            if (context.StartingNode != null)
                context.MainCanvas.Children.Remove(context.StartingNode.startingArrow);
            context.StartingNode = null;
        }
        private static void SetStartingNodeToNode(AutomatonContext context, ModelNode node)
        {
            //Add the starting arrow to the canvas
            context.MainCanvas.Children.Add(node.startingArrow);

            //if there was a starting node, toggle it.
            if (context.StartingNode != null)
                context.ToggleNodeStarting(context.StartingNode);

            //Set this node to be the starting node
            context.StartingNode = node;
        }

        public static void ToggleNodeStarting(this AutomatonContext context, ModelNode node)
        {
            //Toggle the starting flag.
            node.Starting = !node.Starting;

            if (node.Starting)
            {
                SetStartingNodeToNode(context, node);
            }
            else //Remove the starting node of the used-to-be-starting node
            {
                SetStartingNodeToNull(context);
            }
        }
        #endregion

        #region CreateAddRemoveNodeActions
        public static ModelNode CreateAndAddNode(this AutomatonContext context, MainWindow host)
        {
            //Create the new node
            ModelNode node = new(context.NodesList.Count, host, context.LastRightClickPosition);

            //Auto-set the node to the starting node if it's the only one
            if (context.NodesList.Count == 0)
                node.Starting = true;
            //Add the node to the Canvas
            Canvas.SetLeft(node, context.LastRightClickPosition.X - node.Size / 2);
            Canvas.SetTop(node, context.LastRightClickPosition.Y - node.Size / 2);

            context.AddNode(node, host);

            return node;
        }

        public static void AddNode(this AutomatonContext context, ModelNode nodeToAdd ,MainWindow host)
        {
            nodeToAdd.Index = context.NodesList.Count;
            //Add each line connected to the node.
            foreach (object connector in nodeToAdd.connectedLines)
            {
                context.MainCanvas.Children.Add((UIElement)connector);
                context.MainCanvas.Children.Add((UIElement)((Shape)connector).Tag);
            }

            //if the node was the starting node, set it to null and remove the arrow from the screen.
            if (nodeToAdd.Starting)
            {
                SetStartingNodeToNode(context, nodeToAdd);
            }

            context.MainCanvas.Children.Add(nodeToAdd);
            context.NodesList.Add(nodeToAdd);

            //Add the node to each of the context menus of the existing nodes
            context.SetNodesContextMenuOptions(host);
        }

        public static void RemoveNode(this AutomatonContext context, ModelNode nodeToRemove, MainWindow host)
        {
            //Decrease the index of each node that was created after the current node.
            foreach (ModelNode node in context.NodesList)
            {
                if (node.Index > nodeToRemove.Index)
                    node.Index--;
            }

            //Remove each line connected to the node.
            foreach (object connector in nodeToRemove.connectedLines)
            {
                context.MainCanvas.Children.Remove((UIElement)connector);
                context.MainCanvas.Children.Remove((UIElement)((Shape)connector).Tag);
            }

            //if the node was the starting node, set it to null and remove the arrow from the screen.
            if (nodeToRemove.Starting)
            {
                SetStartingNodeToNull(context);
            }

            //Remove the node itself and decrease the amount of nodes on screen.
            context.MainCanvas.Children.Remove(nodeToRemove);
            context.NodesList.Remove(nodeToRemove);

            //Remove the node from the context menu of each existing node.
            context.SetNodesContextMenuOptions(host);
        }
        #endregion

        #region Misc
        public static void SetNodesContextMenuOptions(this AutomatonContext context, MainWindow host)
        {
            foreach (ModelNode changedNode in context.NodesList)
            {
                changedNode.ConnectMenuItem.Items.Clear();
                foreach (ModelNode q in context.NodesList)
                {
                    MenuItem menuItem = new();
                    menuItem.Click += host.ConnectNodes;
                    //Link the node to the MenuItem
                    menuItem.Tag = changedNode;
                    menuItem.Header = q.ToString();
                    changedNode.ConnectMenuItem.Items.Add(menuItem);
                }
            }
        }
        #endregion
    }
}

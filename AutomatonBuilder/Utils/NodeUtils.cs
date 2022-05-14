using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;

namespace AutomatonBuilder.Utils
{
    public static class NodeUtils
    {
        #region StartingNodeActions
        private static void SetStartingNodeToNull(AutomatonContext context)
        {
            if (context.StartingNode != null)
                context.MainCanvas.Children.Remove(context.StartingNode.StartingArrow);
            context.StartingNode = null;
        }
        private static void SetStartingNodeToNode(AutomatonContext context, ModelNode node)
        {
            //Add the starting arrow to the canvas
            context.MainCanvas.Children.Add(node.StartingArrow);

            //if there was a starting node, toggle it.
            if (context.StartingNode != null)
                NodeUtils.ToggleNodeStarting(context, context.StartingNode);

            //Set this node to be the starting node
            context.StartingNode = node;
        }

        public static void ToggleNodeStarting(AutomatonContext context, ModelNode node)
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
        public static ModelNode CreateAndAddNode(AutomatonContext context, MainEditingScreen host)
        {
            //Create the new node
            ModelNode node = new(context.NodesList.Count, host, context.MouseProperies.LastRightClickPosition);

            node.MouseEnter += host.GeneralElement_MouseEnter;
            node.MouseLeave += host.Element_MouseLeave;

            //Auto-set the node to the starting node if it's the only one
            if (context.NodesList.Count == 0)
                node.Starting = true;
            //Add the node to the Canvas

            node.SetPosition(context.MouseProperies.LastRightClickPosition);

            NodeUtils.AddNodeToCanvas(context, node, host);

            return node;
        }

        public static void AddNodeToCanvas(AutomatonContext context, ModelNode nodeToAdd ,MainEditingScreen host)
        {
            nodeToAdd.Index = context.NodesList.Count;

            //Add each line connected to the node.
            foreach (IConnector connector in nodeToAdd.ConnectorsToThisNode.Values)
            {
                connector.AddToCanvasButtom(context.MainCanvas);
            }
            foreach (IConnector connector in nodeToAdd.ConnectorsFromThisNode.Values)
            {
                connector.AddToCanvasButtom(context.MainCanvas);
            }

            //if the node was the starting node, set it to null and remove the arrow from the screen.
            if (nodeToAdd.Starting)
            {
                SetStartingNodeToNode(context, nodeToAdd);
            }

            context.MainCanvas.Children.Add(nodeToAdd);
            context.NodesList.Add(nodeToAdd);

            //Add the node to each of the context menus of the existing nodes
            NodeUtils.SetNodesContextMenuOptions(context, host);
        }

        public static void RemoveNode(AutomatonContext context, ModelNode nodeToRemove, MainEditingScreen host)
        {
            //Decrease the index of each node that was created after the current node.
            foreach (ModelNode node in context.NodesList)
            {
                if (node.Index > nodeToRemove.Index)
                    node.Index--;
            }


            //Remove each line connected to the node.
            foreach (IConnector connector in nodeToRemove.ConnectorsFromThisNode.Values)
            {
                connector.RemoveFromCanvas(context.MainCanvas);
            }

            foreach (IConnector connector in nodeToRemove.ConnectorsToThisNode.Values)
            {
                connector.RemoveFromCanvas(context.MainCanvas);
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
            NodeUtils.SetNodesContextMenuOptions(context, host);
        }
        #endregion

        #region Misc
        public static void SetNodesContextMenuOptions(AutomatonContext context, MainEditingScreen host)
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

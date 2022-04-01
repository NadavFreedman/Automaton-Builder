using AutomatonBuilder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AutomatonBuilder.Actions.NodeActions
{
    public class RemoveNodeAction: IAction
    {
        private readonly AutomatonContext context;
        private readonly MainWindow host;
        private readonly ModelNode node;

        public RemoveNodeAction(AutomatonContext context, MainWindow host, ModelNode node)
        {
            this.context = context;
            this.host = host;
            this.node = node;
        }

        public void DoAction()
        {
            //Decrease the index of each node that was created after the current node.
            for (int i = this.context.MainCanvas.Children.Count - 1; i >= 0; i--)
            {
                object item = this.context.MainCanvas.Children[i];
                if (item is ModelNode itemNode)
                {
                    if (itemNode == node)
                        break;
                    else
                        itemNode.Index--;
                }
            }

            //Remove each line connected to the node.
            foreach (object connector in node.connectedLines)
            {
                this.context.MainCanvas.Children.Remove((UIElement)connector);
                this.context.MainCanvas.Children.Remove((UIElement)((Shape)connector).Tag);
            }

            //if the node was the starting node, set it to null and remove the arrow from the screen.
            if (node.Starting)
            {
                this.context.StartingNode = null;
                this.context.MainCanvas.Children.Remove(node.startingArrow);
            }

            //Remove the node itself and decrease the amount of nodes on screen.
            this.context.MainCanvas.Children.Remove(node);
            this.context.NodesList.Remove(this.context.NodesList[this.context.NodesList.Count - 1]);
            this.context.NodeCount--;

            //Remove the node from the context menu of each existing node.
            foreach (ModelNode existingNode in this.context.NodesList)
            {
                existingNode.ConnectMenuItem.Items.Clear();
                foreach (ModelNode q in this.context.NodesList)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Click += this.host.ConnectNodes;
                    menuItem.Tag = existingNode;
                    menuItem.Header = q.ToString();
                    existingNode.ConnectMenuItem.Items.Add(menuItem);
                }
            }
        }

        public void UndoAction()
        {
            throw new NotImplementedException();
        }
    }
}

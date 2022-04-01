using AutomatonBuilder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutomatonBuilder.Actions.NodeActions
{
    public class AddNodeAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly MainWindow host;

        public AddNodeAction(AutomatonContext context, MainWindow host)
        {
            this.context = context;
            this.host = host;
        }

        public void DoAction()
        {
            //Create the new node
            ModelNode node = new(this.context.NodeCount++, this.host, this.context.LastRightClickPosition);

            //Auto-set the node to the starting node if it's the only one
            if (this.context.NodeCount == 1)
                this.context.ToggleNodeStarting(node);

            //Add the node to the Canvas
            Canvas.SetLeft(node, context.LastRightClickPosition.X - node.Size / 2);
            Canvas.SetTop(node, context.LastRightClickPosition.Y - node.Size / 2);
            this.context.MainCanvas.Children.Add(node);
            this.context.NodesList.Add(node);

            //Add the node to each of the context menus of the existing nodes
            foreach (ModelNode changedNode in this.context.NodesList)
            {
                changedNode.ConnectMenuItem.Items.Clear();
                foreach (ModelNode q in this.context.NodesList)
                {
                    MenuItem menuItem = new();
                    menuItem.Click += this.host.ConnectNodes;
                    //Link the node to the MenuItem
                    menuItem.Tag = changedNode;
                    menuItem.Header = q.ToString();
                    changedNode.ConnectMenuItem.Items.Add(menuItem);
                }
            }
        }

        public void UndoAction()
        {
            throw new NotImplementedException();
        }
    }
}

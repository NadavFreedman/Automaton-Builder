using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
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
            NodeUtils.RemoveNode(this.context, this.node, this.host);
        }

        public void RedoAction()
        {
            NodeUtils.RemoveNode(this.context, this.node, this.host);
        }

        public void UndoAction()
        {
            NodeUtils.AddNode(this.context, this.node!, this.host);
        }
    }
}

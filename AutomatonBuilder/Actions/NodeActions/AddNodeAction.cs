using AutomatonBuilder.Entities;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
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
        private ModelNode? createdNode;

        public AddNodeAction(AutomatonContext context, MainWindow host)
        {
            this.context = context;
            this.host = host;
        }

        public void DoAction()
        {
            this.createdNode = NodeUtils.CreateAndAddNode(this.context, this.host);
        }

        public void RedoAction()
        {
            NodeUtils.AddNode(this.context, this.createdNode!, this.host);
        }

        public void UndoAction()
        {
            NodeUtils.RemoveNode(this.context, this.createdNode!, this.host);
        }
    }
}

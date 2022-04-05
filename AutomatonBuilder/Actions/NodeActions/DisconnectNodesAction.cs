using AutomatonBuilder.Entities;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomatonBuilder.Actions.NodeActions
{
    public class DisconnectNodesAction : IAction
    {
        private AutomatonContext context;
        private readonly IConnector connector;
        private List<ModelNode>? disconnectedNodes;

        public DisconnectNodesAction(AutomatonContext context, IConnector connector)
        {
            this.context = context;
            this.connector = connector;
        }
        public void DoAction()
        {
            this.disconnectedNodes = ConnectorUtils.DisconnectConnector(context, connector);
        }

        public void RedoAction()
        {
            ConnectorUtils.DisconnectConnector(context, connector);
        }

        public void UndoAction()
        {
            if (disconnectedNodes!.Count == 1)
                ConnectorUtils.ReconnectConnector(context, connector, disconnectedNodes[0]);
            else
                ConnectorUtils.ReconnectConnector(context, connector, disconnectedNodes[0], disconnectedNodes[1]);
        }
    }
}

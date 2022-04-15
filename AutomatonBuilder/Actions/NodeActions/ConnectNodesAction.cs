using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
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
    public class ConnectNodesAction: IAction
    {
        private readonly AutomatonContext context;
        private readonly ModelNode sourse;
        private readonly ModelNode destination;
        private readonly string text;
        private readonly MainEditingScreen host;
        private IConnector connector;

        public ConnectNodesAction(AutomatonContext context, ModelNode source, ModelNode destination, string text, MainEditingScreen host)
        {
            this.context = context;
            this.sourse = source;
            this.destination = destination;
            this.text = text;
            this.host = host;
        }

        public void DoAction()
        {
            this.connector = ConnectorUtils.ConnectNodes(this.context, this.sourse, this.destination, this.text, this.host);
        }

        public void RedoAction()
        {
            ConnectorUtils.ReconnectConnector(this.context, this.connector, this.sourse, this.destination);
        }

        public void UndoAction()
        {

            ConnectorUtils.DisconnectConnector(this.context, this.connector);
        }
    }
}

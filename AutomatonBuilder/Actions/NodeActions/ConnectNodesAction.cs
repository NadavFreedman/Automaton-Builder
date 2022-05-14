using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;

namespace AutomatonBuilder.Actions.NodeActions
{
    public class ConnectNodesAction: IAction
    {
        private readonly AutomatonContext context;
        private readonly ModelNode source;
        private readonly ModelNode destination;
        private readonly MainEditingScreen host;
        private IConnector connector;
        public bool CanceledAction { get; set; }

        public ConnectNodesAction(AutomatonContext context, ModelNode source, ModelNode destination, MainEditingScreen host)
        {
            this.context = context;
            this.source = source;
            this.destination = destination;
            this.host = host;
            this.CanceledAction = true;
        }


        public void DoAction()
        {
            IConnectionModal connectorInputWindow = ConnectorUtils.CreateConnectionModal(
                    this.context.type,
                    this.source.ToString(),
                    this.destination.ToString());
            connectorInputWindow.ShowDialog();
            IConnectorData input;
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.ConnectorData!;
            else
                return;

            this.CanceledAction = false;
            this.connector = ConnectorUtils.ConnectNodes(this.context, this.source, this.destination, input, this.host);
        }

        public void RedoAction()
        {
            ConnectorUtils.ReconnectConnector(this.context, this.connector, this.source, this.destination);
        }

        public void UndoAction()
        {

            ConnectorUtils.DisconnectConnector(this.context, this.connector);
        }
    }
}

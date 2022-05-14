using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;

namespace AutomatonBuilder.Actions.NodeActions
{
    public class EditConnectionAction : IAction
    {
        private IConnector? connectionToEdit;
        private IConnectorData? oldConnectorData;
        private IConnectorData? newConnectorData;
        private readonly AutomatonContext context;
        private readonly ModelNode source;
        private readonly ModelNode destination;

        public bool CanceledAction { get; set; }

        public EditConnectionAction(AutomatonContext context, ModelNode connectFrom, ModelNode connectTo)
        {
            this.context = context;
            this.source = connectFrom;
            this.destination = connectTo;

            this.CanceledAction = true;
        }

        public EditConnectionAction(AutomatonContext context, IConnector connectorToEdit)
        {
            this.context = context;
            this.source = this.context.NodesList.Find(x => x.ConnectorsFromThisNode.ContainsValue(connectorToEdit))!;
            this.destination = this.context.NodesList.Find(x => x.ConnectorsToThisNode.ContainsValue(connectorToEdit)) ?? this.source;

            this.CanceledAction = true;
        }

        public void DoAction()
        {
            IConnector connectionToEdit = source.ConnectorsFromThisNode[destination];
            IConnectionModal connectorInputWindow = ConnectorUtils.CreateConnectionModal(this.context.type,
                source.ToString(),
                destination.ToString(),
                connectionToEdit.ConnectorData);
            connectorInputWindow.ShowDialog();

            IConnectorData input;
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.ConnectorData!;
            else
                return;

            this.CanceledAction = false;

            this.connectionToEdit = connectionToEdit;
            this.oldConnectorData = connectionToEdit.ConnectorData;
            this.newConnectorData = input;

            this.connectionToEdit.ChangeConnectorData(this.newConnectorData);
        }

        public void RedoAction()
        {
            this.connectionToEdit!.ChangeConnectorData(this.newConnectorData!);
        }

        public void UndoAction()
        {
            this.connectionToEdit!.ChangeConnectorData(this.oldConnectorData!);
        }
    }
}

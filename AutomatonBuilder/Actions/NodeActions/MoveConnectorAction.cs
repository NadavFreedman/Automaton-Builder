using AutomatonBuilder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Entities.Connectors;
using System.Windows;

namespace AutomatonBuilder.Actions.NodeActions
{
    internal class MoveConnectorAction : IAction
    {
        private NodesConnector connector;
        private Point startingPoint;
        private Point endPoint;

        public MoveConnectorAction(NodesConnector connector, Point startingPoint, Point endPoint)
        {
            this.connector = connector;
            this.startingPoint = startingPoint;
            this.endPoint = endPoint;
        }

        public void DoAction()
        {
            return;
        }

        public void RedoAction()
        {
            this.connector.ConnectorMiddlePoint = this.endPoint;
        }

        public void UndoAction()
        {
            this.connector.ConnectorMiddlePoint = this.startingPoint;
        }
    }
}

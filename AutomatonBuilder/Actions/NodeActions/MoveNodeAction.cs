using AutomatonBuilder.Entities;
using System;
using System.Collections.Generic;
using System.Windows;
using AutomatonBuilder.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Actions.NodeActions
{
    public class MoveNodeAction: IAction
    {
        private ModelNode movedNode;
        private Point startingPoint;
        private Point endPoint;

        public MoveNodeAction(ModelNode movedNode, Point startingPoint, Point endPoint)
        {
            this.movedNode = movedNode;
            this.startingPoint = startingPoint;
            this.endPoint = endPoint;
        }

        public void DoAction()
        {
            return;
        }

        public void RedoAction()
        {
            movedNode.SetPosition(this.endPoint);
        }

        public void UndoAction()
        {
            movedNode.SetPosition(this.startingPoint);
        }
    }
}

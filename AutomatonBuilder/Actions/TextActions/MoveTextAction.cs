using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using System.Windows;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;

namespace AutomatonBuilder.Actions.TextActions
{
    public class MoveTextAction : IAction
    {
        private Point startingPoint;
        private Point endPoint;
        private Border borderedText;

        public MoveTextAction(Border movedBorderedText, Point startingPoint, Point endPoint)
        {
            this.borderedText = movedBorderedText;
            this.startingPoint = startingPoint;
            this.endPoint = endPoint;
        }
        public void DoAction()
        {
            return;
        }

        public void RedoAction()
        {
            TextUtils.SetPositionForText(borderedText, endPoint);
        }

        public void UndoAction()
        {
            TextUtils.SetPositionForText(borderedText, startingPoint);
        }
    }
}

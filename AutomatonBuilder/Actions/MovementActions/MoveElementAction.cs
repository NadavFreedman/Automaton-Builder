using AutomatonBuilder.Interfaces;
using System.Windows;

namespace AutomatonBuilder.Actions.MovementActions
{
    public class MoveElementAction : IAction
    {
        private readonly IMoveable movedElement;
        private Point leftClickHoldStartingPosition;
        private Point leftClickHoldReleasePosition;

        public MoveElementAction(IMoveable borderedText, Point leftClickHoldStartingPosition, Point leftClickHoldReleasePosition)
        {
            this.movedElement = borderedText;
            this.leftClickHoldStartingPosition = leftClickHoldStartingPosition;
            this.leftClickHoldReleasePosition = leftClickHoldReleasePosition;
        }

        public void DoAction()
        {
            return;
        }

        public void RedoAction()
        {
            this.movedElement.SetPosition(this.leftClickHoldReleasePosition);
        }

        public void UndoAction()
        {
            this.movedElement.SetPosition(this.leftClickHoldStartingPosition);
        }
    }
}

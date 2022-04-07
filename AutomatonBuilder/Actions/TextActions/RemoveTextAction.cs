using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;
using AutomatonBuilder.Entities.TextElements;

namespace AutomatonBuilder.Actions.TextActions
{
    public class RemoveTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly BorderedText text;

        public RemoveTextAction(AutomatonContext context, BorderedText text)
        {
            this.context = context;
            this.text = text;
        }

        public void DoAction()
        {
            this.text!.RemoveFromCanvas(this.context.MainCanvas);
        }

        public void RedoAction()
        {
            this.text!.RemoveFromCanvas(this.context.MainCanvas);
        }

        public void UndoAction()
        {
            this.text!.AddToCanvas(this.context.MainCanvas);
        }
    }
}

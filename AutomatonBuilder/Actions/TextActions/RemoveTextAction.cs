using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Entities.Contexts;

namespace AutomatonBuilder.Actions.TextActions
{
    public class RemoveTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly BorderedText borderedText;

        public RemoveTextAction(AutomatonContext context, BorderedText text)
        {
            this.context = context;
            this.borderedText = text;
        }

        public void DoAction()
        {
            this.borderedText!.RemoveFromCanvas(this.context.MainCanvas);
            this.context.BorderedTextsList.Remove(this.borderedText);
        }

        public void RedoAction()
        {
            this.borderedText!.RemoveFromCanvas(this.context.MainCanvas);
            this.context.BorderedTextsList.Remove(this.borderedText);
        }

        public void UndoAction()
        {
            this.borderedText!.AddToCanvas(this.context.MainCanvas);
            this.context.BorderedTextsList.Add(this.borderedText);
        }
    }
}

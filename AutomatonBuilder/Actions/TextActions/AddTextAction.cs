using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Entities.Contexts;

namespace AutomatonBuilder.Actions.TextActions
{
    public class AddTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly string text;
        private readonly MainWindow host;
        private BorderedText? borderedText;

        public AddTextAction(AutomatonContext context, string text, MainWindow host)
        {
            this.context = context;
            this.text = text;
            this.host = host;
        }
        public void DoAction()
        {
            //Create a text block
            this.borderedText = new(this.text);
            this.context.BorderedTextsList.Add(this.borderedText);


            //Add a context menu to the text block
            TextUtils.AddContextMenuToText(this.borderedText, host);

            this.borderedText.AllowDragging(host);

            this.borderedText.SetPosition(context.MouseProperies.LastRightClickPosition);

            borderedText.AddToCanvas(context.MainCanvas);
        }

        public void RedoAction()
        {
            this.borderedText!.AddToCanvas(this.context.MainCanvas);
            this.context.BorderedTextsList.Add(this.borderedText);
        }

        public void UndoAction()
        {
            this.borderedText!.RemoveFromCanvas(this.context.MainCanvas);
            this.context.BorderedTextsList.Remove(this.borderedText);
        }
    }
}

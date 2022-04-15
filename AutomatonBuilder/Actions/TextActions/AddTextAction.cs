using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Entities.Args;

namespace AutomatonBuilder.Actions.TextActions
{
    public class AddTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly AddTextArgs args;
        private readonly MainEditingScreen host;
        private BorderedText? borderedText;

        public AddTextAction(AutomatonContext context, AddTextArgs args, MainEditingScreen host)
        {
            this.context = context;
            this.args = args;
            this.host = host;
        }
        public void DoAction()
        {
            //Create a text block
            this.borderedText = new(this.args);
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

using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;
using AutomatonBuilder.Entities.TextElements;

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

            //Add a context menu to the text block
            var contextMenu = new ContextMenu();
            MenuItem removeTextItem = new MenuItem
            {
                Header = "Remove",
                Tag = borderedText
            };
            removeTextItem.Click += this.host.RemoveText_Click;
            contextMenu.Items.Add(removeTextItem);

            this.borderedText.AttachContextMenu(contextMenu);

            this.borderedText.AllowDragging(host);

            this.borderedText.SetPosition(context.MouseProperies.LastRightClickPosition);

            borderedText.AddToCanvas(context.MainCanvas);
        }

        public void RedoAction()
        {
            this.borderedText!.AddToCanvas(this.context.MainCanvas);
        }

        public void UndoAction()
        {
            this.borderedText!.RemoveFromCanvas(this.context.MainCanvas);
        }
    }
}

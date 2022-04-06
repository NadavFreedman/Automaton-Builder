using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;

namespace AutomatonBuilder.Actions.TextActions
{
    public class AddTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly string text;
        private readonly MainWindow host;
        private Border? borderedText;

        public AddTextAction(AutomatonContext context, string text, MainWindow host)
        {
            this.context = context;
            this.text = text;
            this.host = host;
        }
        public void DoAction()
        {
            //Create a text block
            Border borderedText = TextUtils.CreateBorderWithTextBlock(this.text);

            //Calculate the size of the text block
            

            //Add a context menu to the text block
            borderedText.ContextMenu = new ContextMenu();
            MenuItem removeTextItem = new MenuItem
            {
                Header = "Remove",
                Tag = borderedText
            };
            removeTextItem.Click += this.host.RemoveText_Click;
            borderedText.ContextMenu.Items.Add(removeTextItem);

            ((TextBlock)borderedText.Child).MouseLeave += host.Element_MouseLeave;
            ((TextBlock)borderedText.Child).MouseEnter += host.GeneralElement_MouseEnter;

            TextUtils.SetPositionForText(borderedText, context.LastRightClickPosition);

            this.borderedText = borderedText;
            context.MainCanvas.Children.Add(borderedText);
        }

        public void RedoAction()
        {
            TextUtils.AddBorderedElementToCanvas(this.borderedText!, this.context.MainCanvas);
        }

        public void UndoAction()
        {
            TextUtils.RemoveBorderedElementFromCanvas(this.borderedText!, this.context.MainCanvas);

        }
    }
}

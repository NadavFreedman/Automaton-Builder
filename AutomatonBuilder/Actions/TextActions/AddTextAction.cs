using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.Actions
{
    public class AddTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly string text;
        private readonly MainWindow host;
        private Border? textBox;

        public AddTextAction(AutomatonContext context, string text, MainWindow host)
        {
            this.context = context;
            this.text = text;
            this.host = host;
        }
        public void DoAction()
        {
            //Create a text block
            Border border = TextUtils.CreateBorderWithTextBlock(this.text);

            //Calculate the size of the text block
            var formattedText = TextUtils.CreateFormattedText((TextBlock)border.Child);

            //Add a context menu to the text block
            border.ContextMenu = new ContextMenu();
            MenuItem removeTextItem = new MenuItem
            {
                Header = "Remove",
                Tag = border
            };
            removeTextItem.Click += this.host.RemoveText_Click;
            border.ContextMenu.Items.Add(removeTextItem);

            ((TextBlock)border.Child).MouseLeave += host.Element_MouseLeave;
            ((TextBlock)border.Child).MouseEnter += host.Element_MouseEnter;

            //Set the block to the canvas
            Canvas.SetLeft(border, context.LastRightClickPosition.X - 2 - formattedText.Width / 2);
            Canvas.SetTop(border, context.LastRightClickPosition.Y - 2 - formattedText.Height / 2);

            this.textBox = border;
            context.MainCanvas.Children.Add(border);
        }

        public void RedoAction()
        {
            TextUtils.AddTextBoxToCanvas(this.textBox!, this.context.MainCanvas);
        }

        public void UndoAction()
        {
            TextUtils.RemoveTextBoxFromCanvas(this.textBox!, this.context.MainCanvas);

        }
    }
}

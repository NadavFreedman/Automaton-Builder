using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.TextElements
{
    public class BorderedText: IMoveable
    {
        public Border Border { get; private set; }
        private readonly TextBlock block;
        private FormattedText formattedText;
        private Point position;

        public BorderedText(string text)
        {
            this.Border = TextUtils.CreateBorderWithTextBlock(text);
            this.block = (TextBlock)this.Border.Tag;
            this.formattedText = TextUtils.CreateFormattedText(this.block);
            this.block.Tag = this;
        }

        public void AllowDragging(MainWindow mainWindow)
        {
            block.MouseEnter += mainWindow.TaggedElement_MouseEnter;
            block.MouseLeave += mainWindow.Element_MouseLeave;
        }

        public void SetPosition(Point newPosition)
        {
            this.position = newPosition;
            Canvas.SetLeft(this.Border, newPosition.X - 2 - this.formattedText.Width / 2);
            Canvas.SetTop(this.Border, newPosition.Y - 2 - this.formattedText.Height / 2);
        }

        public void SetText(string text)
        {
            this.block.Text = text;
            this.formattedText = TextUtils.CreateFormattedText(this.block);
        }

        public string GetText()
        {
            return this.block.Text;
        }

        public void AddToCanvas(Canvas mainCanvas, ZAxis z = ZAxis.Top)
        {
            if (z == ZAxis.Top)
                mainCanvas.Children.Add(this.Border);
            else
                mainCanvas.Children.Insert(0, this.Border);
        }

        public void RemoveFromCanvas(Canvas mainCanvas)
        {
            mainCanvas.Children.Remove(this.Border);
        }

        internal void AttachContextMenu(ContextMenu menu)
        {
            this.block.ContextMenu = menu;
        }

        public Point GetPosition()
        {
            return this.position;
        }
    }
}

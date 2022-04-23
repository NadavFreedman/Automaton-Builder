using AutomatonBuilder.Entities.Args;
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
        public TextBlock Block { get; }
        private FormattedText formattedText;
        private Point position;

        public BorderedText(string text)
        {
            this.Border = TextUtils.CreateBorderWithTextBlock(text);
            this.Block = (TextBlock)this.Border.Tag;
            this.formattedText = TextUtils.CreateFormattedText(this.Block);
            this.Block.Tag = this;
        }

        public BorderedText(AddTextArgs args)
        {
            this.Border = TextUtils.CreateBorderWithTextBlock(args);
            this.Block = (TextBlock)this.Border.Tag;
            this.formattedText = TextUtils.CreateFormattedText(this.Block);
            this.Block.Tag = this;
        }

        public void AllowDragging(MainEditingScreen mainWindow)
        {
            Block.MouseEnter += mainWindow.TaggedElement_MouseEnter;
            Block.MouseLeave += mainWindow.Element_MouseLeave;
        }

        public void SetPosition(Point newPosition)
        {
            this.position = newPosition;
            Canvas.SetLeft(this.Border, newPosition.X - 2 - this.formattedText.Width / 2);
            Canvas.SetTop(this.Border, newPosition.Y - 2 - this.formattedText.Height / 2);
        }

        public void SetText(string text)
        {
            this.Block.Text = text;
            this.formattedText = TextUtils.CreateFormattedText(this.Block);
        }

        public string GetText()
        {
            return this.Block.Text;
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
            this.Block.ContextMenu = menu;
        }

        public Point GetPosition()
        {
            return this.position;
        }

        public double Width { get { return this.formattedText.Width + this.Border.BorderThickness.Right + this.Border.BorderThickness.Left; } }

        public double Height { get { return this.formattedText.Height + this.Border.BorderThickness.Top + this.Border.BorderThickness.Bottom; } }

    }
}

using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutomatonBuilder.Entities.Graphics.Memories
{
    public class GraphicalStackMemory : IMoveable
    {
        private Border graphicalMemory;
        private Point position;
        private Canvas? canvas;
        private MainEditingScreen? mainEditingScreen;

        public GraphicalStackMemory(Stack<char> stack, Point position)
        {
            this.graphicalMemory = RunningUtils.CreateGraphicalStackMemory(stack);
            this.SetPosition(position);
        }

        public void AddToCanvas(Canvas canvas)
        {
            if (canvas.Children.Contains(this.graphicalMemory)) return;

            this.canvas = canvas;

            if (this.graphicalMemory.Parent != null)
                ((Canvas)this.graphicalMemory.Parent).Children.Remove(this.graphicalMemory);

            this.canvas.Children.Add(this.graphicalMemory);
            this.SetPosition(this.position);
        }

        public void UpdateStack(Stack<char> stack)
        {
            this.canvas!.Children.Remove(this.graphicalMemory);
            this.graphicalMemory = RunningUtils.CreateGraphicalStackMemory(stack);
            this.canvas.Children.Add(this.graphicalMemory);
            this.mainEditingScreen!.UpdateLayout();
            this.SyncPosition();
            this.SyncDragging();
        }

        public Point GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Point newPosition)
        {
            this.position = newPosition;
            this.SyncPosition();
        }

        public void SyncPosition()
        {
            Canvas.SetLeft(this.graphicalMemory, this.position.X - this.graphicalMemory.ActualWidth / 2);
            Canvas.SetTop(this.graphicalMemory, this.position.Y - this.graphicalMemory.ActualHeight / 2);
        }

        public void AllowDragging(MainEditingScreen mainScreen)
        {
            this.mainEditingScreen = mainScreen;
            this.SyncDragging();
        }

        public void SyncDragging()
        {
            this.graphicalMemory.Tag = this;
            this.graphicalMemory.MouseEnter += this.mainEditingScreen.TaggedElement_MouseEnter;
            this.graphicalMemory.MouseLeave += this.mainEditingScreen.Element_MouseLeave;
        }
    }
}

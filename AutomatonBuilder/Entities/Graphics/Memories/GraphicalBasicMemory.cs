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
    public class GraphicalBasicMemory: IMoveable
    {
        private Border graphicalMemory;
        private Point position;

        public GraphicalBasicMemory(string word, int currentIndex, Point position)
        {
            this.graphicalMemory = RunningUtils.CreateGraphicalBasicMemory(word, currentIndex);
            this.position = position;
            this.SetPosition(this.position);
        }

        public void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(this.graphicalMemory);
        }

        public void ChangeWord(string word, int currentIndex)
        {
            this.graphicalMemory = RunningUtils.CreateGraphicalBasicMemory(word, currentIndex);
            this.SetPosition(this.position);
        }

        public Point GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Point newPosition)
        {
            this.position = newPosition;
            Canvas.SetLeft(this.graphicalMemory, newPosition.X - this.graphicalMemory.Width / 2);
            Canvas.SetTop(this.graphicalMemory, newPosition.Y - this.graphicalMemory.Height / 2);
        }

        public void AllowDragging(MainEditingScreen mainWindow)
        {
            this.graphicalMemory.MouseEnter += mainWindow.GeneralElement_MouseEnter;
            this.graphicalMemory.MouseLeave += mainWindow.Element_MouseLeave;
        }
    }
}

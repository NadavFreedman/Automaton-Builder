using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatonBuilder.Interfaces;
using System.Windows.Shapes;
using System.Windows.Media;
using AutomatonBuilder.Entities;

namespace AutomatonBuilder.Actions.DrawingActions
{
    public class DrawLineAction : IAction
    {
        private readonly Polyline drawnLine;
        private readonly AutomatonContext context;

        public DrawLineAction(AutomatonContext context)
        {
            this.context = context;
            this.drawnLine = context.CurrentLine!;
        }
        public void DoAction()
        {
            this.context.CurrentLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2.5,
                Points = new PointCollection()
            };
            this.context.MainCanvas.Children.Add(this.context.CurrentLine);
        }

        public void RedoAction()
        {
            this.context.MainCanvas.Children.Add(this.drawnLine);
        }

        public void UndoAction()
        {
            this.context.MainCanvas.Children.Remove(this.drawnLine);
        }
    }
}

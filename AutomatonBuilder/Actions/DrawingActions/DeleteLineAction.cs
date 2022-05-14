using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Interfaces;
using System;
using System.Windows.Shapes;

namespace AutomatonBuilder.Actions.DrawingActions
{
    public class DeleteLineAction : IAction
    {
        private readonly Polyline line;
        private readonly AutomatonContext context;
        public bool CanceledAction { get; set; } = false;

        public DeleteLineAction(AutomatonContext context, Polyline line)
        {
            this.line = line;
            this.context = context;
        }

        public void DoAction()
        {
            this.context.MainCanvas.Children.Remove(line);
            this.context.DrawnLinesList.Remove(line);
        }

        public void RedoAction()
        {
            this.context.MainCanvas.Children.Remove(line);
            this.context.DrawnLinesList.Remove(line);
        }

        public void UndoAction()
        {
            this.context.MainCanvas.Children.Add(line);
            this.context.DrawnLinesList.Add(line);
        }
    }
}

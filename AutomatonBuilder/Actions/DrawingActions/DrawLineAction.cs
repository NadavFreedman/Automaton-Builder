using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace AutomatonBuilder.Entities.Actions
{
    public class DrawLineAction : IAction
    {
        private readonly List<Ellipse> drawnLine;
        private readonly AutomatonContext context;

        public DrawLineAction(AutomatonContext context)
        {
            this.context = context;
            this.drawnLine = context.CurrentLine!;
        }
        public void DoAction()
        {
            return;
        }

        public void RedoAction()
        {
            foreach (Ellipse dotToAdd in this.drawnLine)
                context.MainCanvas.Children.Add(dotToAdd);
        }

        public void UndoAction()
        {
            foreach (Ellipse dotToRemove in this.drawnLine)
                context.MainCanvas.Children.Remove(dotToRemove);
        }
    }
}

using AutomatonBuilder.Entities;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Interfaces;
using System.Windows.Controls;

namespace AutomatonBuilder.Actions.TextActions
{
    public class RemoveTextAction : IAction
    {
        private readonly AutomatonContext context;
        private readonly Border box;

        public RemoveTextAction(AutomatonContext context, Border box)
        {
            this.context = context;
            this.box = box;
        }

        public void DoAction()
        {
            TextUtils.RemoveBorderedElementFromCanvas(this.box!, this.context.MainCanvas);
        }

        public void RedoAction()
        {
            TextUtils.RemoveBorderedElementFromCanvas(this.box!, this.context.MainCanvas);
        }

        public void UndoAction()
        {
            TextUtils.AddBorderedElementToCanvas(this.box!, this.context.MainCanvas);
        }
    }
}

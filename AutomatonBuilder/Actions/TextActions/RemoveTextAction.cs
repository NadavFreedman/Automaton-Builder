using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutomatonBuilder.Entities.Actions.TextActions
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
            TextUtils.RemoveTextBoxFromCanvas(this.box!, this.context.MainCanvas);
        }

        public void RedoAction()
        {
            TextUtils.RemoveTextBoxFromCanvas(this.box!, this.context.MainCanvas);
        }

        public void UndoAction()
        {
            TextUtils.AddTextBoxToCanvas(this.box!, this.context.MainCanvas);
        }
    }
}

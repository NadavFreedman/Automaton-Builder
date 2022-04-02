using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Entities
{
    public interface IAction
    {
        public void DoAction();

        public void RedoAction();

        public void UndoAction();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Interfaces
{
    public interface IAction
    {
        public bool CanceledAction { get; set; }
        public void DoAction();

        public void RedoAction();

        public void UndoAction();
    }
}

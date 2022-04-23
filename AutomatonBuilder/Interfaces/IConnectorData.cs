using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Interfaces
{
    public interface IConnectorData
    {
        public bool ShouldMove(IAutomatonMemory memory);

        public IAutomatonMemory OnMovementAction(IAutomatonMemory memory);
    }
}

using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Entities.Connectors.ConnectorData
{
    public class TuringAutomatonData : IConnectorData
    {
        public List<SingleTuringData> Data { get; set; }

        public TuringAutomatonData() { }

        public TuringAutomatonData(List<SingleTuringData> data)
        {
            this.Data = data;
        }
        public IAutomatonMemory OnMovementAction(IAutomatonMemory memory)
        {
            SingleTuringData matchingData = this.Data.Find(single => single.MatchesCondition((TuringMemory)memory))!;

            TuringMemory clonedMemory = (TuringMemory)memory.Clone();
            clonedMemory.WriteAndMove(matchingData.WriteActionValue, matchingData.Action);
            return clonedMemory;
        }

        public bool ShouldMove(IAutomatonMemory memory)
        {
            return this.Data.Any(single => single.MatchesCondition((TuringMemory)memory));
        }

        public override string ToString()
        {
            return string.Join("\n", this.Data.Select(single => single.ToString()));
        }
    }
}

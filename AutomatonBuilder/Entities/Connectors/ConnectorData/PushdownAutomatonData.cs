using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AutomatonBuilder.Entities.Connectors.ConnectorData
{
    public class PushdownAutomatonData : IConnectorData
    {
        public List<SinglePushdownData> Data { get; set; }

        public PushdownAutomatonData() { }

        public PushdownAutomatonData(List<SinglePushdownData> data)
        {
            this.Data = data;
        }

        public bool ShouldMove(IAutomatonMemory memory)
        {
            if (memory.IsLastCharacter()) return false;
            return this.Data.Any(single => single.MatchesCondition((StackMemory)memory));
        }

        public IAutomatonMemory OnMovementAction(IAutomatonMemory memory)
        {
            SinglePushdownData matchingData = this.Data.Find(single => single.MatchesCondition((StackMemory)memory))!;

            StackMemory clonedMemory = (StackMemory)memory.Clone();
            switch (matchingData.Action)
            {
                case PushdownActions.Push:
                    clonedMemory.Push(matchingData.ActionData!.Value);
                    break;
                case PushdownActions.Pop:
                    clonedMemory.Pop();
                    break;
            }
            clonedMemory.CurrentIndex++;
            return clonedMemory;
        }

        public override string ToString()
        {
            return string.Join("\n", this.Data.Select(single => single.ToString()));
        }
    }
}

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
    public class BasicAutomatonData : IConnectorData
    {
        public List<SingleBasicData> Data { get; set; }

        public BasicAutomatonData() { }

        public BasicAutomatonData(List<SingleBasicData> data)
        {
            this.Data = data;
        }

        public BasicAutomatonData(List<char> data)
        {
            this.Data = new List<SingleBasicData>();
            foreach (char charData in data)
            {
                this.Data.Add(new SingleBasicData(charData));
            }
        }

        public BasicAutomatonData(string data)
        {
            this.Data = new List<SingleBasicData>();
            foreach (string charData in data.Split(','))
            {
                this.Data.Add(new SingleBasicData(charData.First()));
            }
        }

        public IAutomatonMemory OnMovementAction(IAutomatonMemory memory)
        {
            BasicMemory cloned = (BasicMemory)memory.Clone();
            cloned.CurrentIndex++;
            return cloned;
        }

        public bool ShouldMove(IAutomatonMemory memory)
        {
            if (memory.IsLastCharacter()) return false;
            return Data.Any(single => single.MatchesCondition((BasicMemory)memory));
        }

        public override string ToString()
        {
            return string.Join(",", Data);
        }
    }
}

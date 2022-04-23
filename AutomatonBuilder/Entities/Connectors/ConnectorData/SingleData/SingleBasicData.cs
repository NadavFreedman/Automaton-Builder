using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData
{
    public class SingleBasicData
    {
        public char NeededWordValue { get; set; }

        public SingleBasicData() { }

        public SingleBasicData(char data)
        {
            this.NeededWordValue = data;
        }

        public bool MatchesCondition(BasicMemory memory)
        {
            return memory.GetCurrentValue() == this.NeededWordValue;
        }

        public override string ToString()
        {
            return NeededWordValue.ToString();
        }
    }
}

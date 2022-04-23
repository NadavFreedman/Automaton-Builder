using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Enums;
using System;

namespace AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData
{
    public class SingleTuringData
    {
        public char NeededValue { get; set; }
        public TuringActions Action { get; set; }
        public char WriteActionValue { get; set; }

        public SingleTuringData() { }

        public SingleTuringData(char readValue, char writeValue, TuringActions action)
        {
            this.NeededValue = readValue;
            this.WriteActionValue = writeValue;
            this.Action = action;
        }

        public bool MatchesCondition(TuringMemory memory)
        {
            return memory.CurrentNode.Value == this.NeededValue;
        }

        public override string ToString()
        {
            string direction = "";
            switch (this.Action)
            {
                case TuringActions.MoveRight:
                    direction = "🡆";
                    break;
                case TuringActions.MoveLeft:
                    direction = "🡄";
                    break;
                default:
                    break;
            }
            return $"{this.NeededValue}/{this.WriteActionValue},{direction}";
        }
    }
}

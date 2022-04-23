using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Enums;
using System;

namespace AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData
{
    public class SinglePushdownData
    {
        public char NeededWordValue { get; set; }
        public char NeededStackValue { get; set; }
        public PushdownActions Action { get; set; }
        public char? ActionData { get; set; }

        public SinglePushdownData() { }
        public SinglePushdownData(char wordValue, char stackValue, PushdownActions action, char? actionData = null)
        {
            this.NeededWordValue = wordValue;
            this.NeededStackValue = stackValue;
            this.Action = action;
            this.ActionData = actionData;
            if (this.ActionData is null && this.Action == PushdownActions.Push)
                throw new ApplicationException("No char has been provided for the push action.");
        }

        public bool MatchesCondition(StackMemory memory)
        {
            return memory.GetCurrentWordValue() == this.NeededWordValue && memory.GetTop() == this.NeededStackValue;
        }

        public override string ToString()
        {
            string actionString = "";
            switch (Action)
            {
                case PushdownActions.Push:
                    actionString = $"▼{this.ActionData}";
                    break;
                case PushdownActions.Pop:
                    actionString = $"▲{this.ActionData}";
                    break;
                case PushdownActions.NoChange:
                    actionString = "NC";
                    break;
                default:
                    break;
            }
            return $"{this.NeededWordValue},{this.NeededStackValue}/{actionString}";
        }

    }
}

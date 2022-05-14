using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.Graphics.Memories;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AutomatonBuilder.Entities.AutomatonMemories
{
    public class TuringMemory : IAutomatonMemory
    {
        private readonly LinkedList<char> memory;
        private int currentIndex;
        private readonly GraphicalTuringMemory graphicalMemory;

        public LinkedListNode<char> CurrentNode { get; set; }

        public TuringMemory(string word, GraphicalTuringMemory graphicalMemory)
        {
            this.memory = new LinkedList<char>(word);
            this.CurrentNode = new LinkedList<char>(word).First!;
            this.currentIndex = 1;
            this.memory.AddFirst('├');
            this.graphicalMemory = graphicalMemory;
        }

        public TuringMemory(TuringMemory origin)
        {
            this.memory = new LinkedList<char>(origin.memory);
            this.currentIndex = origin.currentIndex;
            this.CurrentNode = this.memory.First;
            for (int i = 0; i < this.currentIndex; i++)
                this.CurrentNode = this.CurrentNode!.Next;
            this.graphicalMemory = origin.graphicalMemory;
        }

        public IAutomatonMemory Clone()
        {
            return new TuringMemory(this);
        }

        public void WriteAndMove(char newValue, TuringActions direction)
        {
            this.CurrentNode.Value = newValue;
            if (direction == TuringActions.MoveRight)
            {
                if (this.CurrentNode.Next == null)
                    this.memory.AddAfter(this.CurrentNode, '△');
                this.CurrentNode = this.CurrentNode.Next;
                this.currentIndex++;
            }
            else
            {
                if (this.CurrentNode.Previous!.Value == '├')
                    throw new Exception("Tried to move out of turing list bounds");
                this.CurrentNode = this.CurrentNode.Previous;
                this.currentIndex--;
            }
        }

        public bool IsLastCharacter()
        {
            return true;
        }

        public bool IsDetermenistic()
        {
            return true;
        }

        public bool IsFinishableWord()
        {
            return false;
        }

        public void PrintMemroy()
        {
            graphicalMemory.ChangeWord(memory, this.currentIndex);
        }
    }
}

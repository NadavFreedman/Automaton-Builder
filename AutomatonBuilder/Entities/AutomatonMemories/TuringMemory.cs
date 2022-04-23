using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AutomatonBuilder.Entities.AutomatonMemories
{
    public class TuringMemory : IAutomatonMemory
    {
        private LinkedList<char > memory;
        public LinkedListNode<char> CurrentNode { get; set; }

        public TuringMemory(string word)
        {
            this.memory = new LinkedList<char>(word);
            this.CurrentNode = new LinkedList<char>(word).First!;
            this.memory.AddFirst('├');
        }

        public TuringMemory(LinkedListNode<char> currentNode)
        {
            this.CurrentNode = currentNode;
            this.memory = this.CurrentNode.List!;
        }

        public IAutomatonMemory Clone()
        {
            var list = new LinkedList<char>(this.memory);
            var currentIndex = IndexOf(this.memory, this.CurrentNode);
            var currentNode = list.First;
            for (int i = 0; i < currentIndex; i++)
                currentNode = currentNode!.Next;

            return new TuringMemory(currentNode!);
        }

        public void WriteAndMove(char newValue, TuringActions direction)
        {
            this.CurrentNode.Value = newValue;
            if (direction == TuringActions.MoveRight)
            {
                if (this.CurrentNode.Next == null)
                    this.memory.AddAfter(this.CurrentNode, '△');
                this.CurrentNode = this.CurrentNode.Next;
            }
            else
            {
                if (this.CurrentNode.Previous!.Value == '├')
                    throw new Exception("Tried to move out of turing list bounds");
                this.CurrentNode = this.CurrentNode.Previous;
            }
        }

        public static int IndexOf<T>(LinkedList<T> list, LinkedListNode<T> nodeToFind)
        {
            var count = 0;
            for (var node = list.First; node != null; node = node.Next, count++)
            {
                if (nodeToFind == node)
                    return count;
            }
            return -1;
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

        public void PrintMemroy(Canvas memoryCanvas)
        {
            throw new NotImplementedException();
        }
    }
}

using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutomatonBuilder.Entities.AutomatonMemories
{
    public class StackMemory : IAutomatonMemory
    {
        private Stack<char> stackMemory;
        private readonly string word;
        public int CurrentIndex { get; set; }

        public StackMemory(string word, int index = 0, Stack<char> memory = null)
        {
            if (memory is null)
                this.stackMemory = new Stack<char>();
            else
                this.stackMemory = new Stack<char>(new Stack<char>(memory));
            this.word = word;
            this.CurrentIndex = index;
        }


        public char GetCurrentWordValue()
        {
            return this.word[CurrentIndex];
        }

        public char GetTop()
        {
            if (this.stackMemory.Any())
                return this.stackMemory.Peek();
            return '⟂';
        }

        public void Push(char data)
        {
            this.stackMemory.Push(data);
        }

        public void Pop()
        {
            this.stackMemory.Pop();
        }

        public bool IsLastCharacter()
        {
            return this.CurrentIndex >= this.word.Length;
        }

        public IAutomatonMemory Clone()
        {
            return new StackMemory(word, this.CurrentIndex, this.stackMemory);
        }

        public bool IsDetermenistic()
        {
            return false;
        }

        public bool IsFinishableWord()
        {
            return true;
        }

        public void PrintMemroy(Canvas memoryCanvas)
        {
            memoryCanvas.Children.Clear();
            Border basicContainer = RunningUtils.CreateGraphicalBasicMemory(this.word, this.CurrentIndex);
            Border stackContainer = RunningUtils.CreateGraphicalStackMemory(this.stackMemory);
            Canvas.SetLeft(stackContainer, 1000);
            memoryCanvas.Children.Add(basicContainer);
            memoryCanvas.Children.Add(stackContainer);
        }
    }
}

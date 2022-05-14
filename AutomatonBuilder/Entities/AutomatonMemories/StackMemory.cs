using AutomatonBuilder.Entities.Graphics.Memories;
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

        private readonly GraphicalBasicMemory graphicalWord;
        private readonly GraphicalStackMemory graphicalStack;


        public int CurrentIndex { get; set; }

        public StackMemory(string word, GraphicalBasicMemory graphicalWord, GraphicalStackMemory graphicalStack)
        {
            this.stackMemory = new Stack<char>();
            this.word = word;
            this.CurrentIndex = 0;
            this.graphicalWord = graphicalWord;
            this.graphicalStack = graphicalStack;
        }

        public StackMemory(StackMemory origin)
        {
            this.stackMemory = new Stack<char>(new Stack<char>(origin.stackMemory));
            this.word = origin.word;
            this.CurrentIndex = origin.CurrentIndex;
            this.graphicalWord = origin.graphicalWord;
            this.graphicalStack = origin.graphicalStack;
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
            return new StackMemory(this);
        }

        public bool IsDetermenistic()
        {
            return false;
        }

        public bool IsFinishableWord()
        {
            return true;
        }

        public void PrintMemroy()
        {
            this.graphicalStack.UpdateStack(this.stackMemory);
            this.graphicalWord.ChangeWord(this.word, this.CurrentIndex);
        }
    }
}

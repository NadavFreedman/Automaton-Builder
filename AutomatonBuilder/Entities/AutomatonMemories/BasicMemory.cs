using AutomatonBuilder.Entities.Graphics.Memories;
using AutomatonBuilder.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace AutomatonBuilder.Entities.AutomatonMemories
{
    public class BasicMemory: IAutomatonMemory
    {
        private readonly string word;
        public int CurrentIndex { get; set; }

        private readonly GraphicalBasicMemory graphicalWord;

        public BasicMemory(string word, GraphicalBasicMemory graphicalMemory)
        {
            this.word = word;
            this.CurrentIndex = 0;
            this.graphicalWord = graphicalMemory;
        }

        public BasicMemory(BasicMemory origin)
        {
            this.word = origin.word;
            this.CurrentIndex = origin.CurrentIndex;
            this.graphicalWord = origin.graphicalWord;
        }

        public char GetCurrentValue()
        {
            return word[CurrentIndex];
        }

        public bool IsLastCharacter()
        {
            return this.CurrentIndex >= this.word.Length;
        }

        public IAutomatonMemory Clone()
        {
            return new BasicMemory(this);
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
            this.graphicalWord.ChangeWord(this.word, this.CurrentIndex);
        }
    }
}

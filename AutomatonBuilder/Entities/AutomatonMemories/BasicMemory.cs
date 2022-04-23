using AutomatonBuilder.Entities.Graphics.Memories;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.AutomatonMemories
{
    public class BasicMemory: IAutomatonMemory
    {
        private readonly string word;
        public int CurrentIndex { get; set; }

        private readonly GraphicalBasicMemory graphicalWord;

        public BasicMemory(string word, int currentIndex = 0)
        {
            this.word = word;
            this.CurrentIndex = currentIndex;
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
            return new BasicMemory(word, this.CurrentIndex);
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
            Border border = RunningUtils.CreateGraphicalBasicMemory(this.word, this.CurrentIndex);
            memoryCanvas.Children.Add(border);
        }
    }
}

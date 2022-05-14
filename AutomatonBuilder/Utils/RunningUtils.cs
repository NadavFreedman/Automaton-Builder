using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.Graphics.Memories;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AutomatonBuilder.Utils
{
    public static class RunningUtils
    {
        public static Border CreateGraphicalBasicMemory(string word, int currentIndex)
        {
            Border border = new();
            TextBlock container = new();
            Run previous = TextUtils.CreateRunElement(word[..currentIndex], 30, Brushes.Gray);
            container.Inlines.Add(previous);
            container.VerticalAlignment = VerticalAlignment.Center;
            container.Background = Brushes.White;

            if (currentIndex < word.Length)
            {
                Run current = TextUtils.CreateRunElement(word[currentIndex].ToString(), 60, Brushes.LawnGreen);
                Run next = TextUtils.CreateRunElement(word[(currentIndex + 1)..], 40, Brushes.Black);
                current.FontWeight = FontWeights.Bold;
                container.Inlines.Add(current);
                container.Inlines.Add(next);
            }

            border.Child = container;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;

            return border;
        }

        public static Border CreateGraphicalStackMemory(Stack<char> stack, int maxInStack = 6)
        {
            Border border = new();
            StackPanel stackContainer = new();
            stackContainer.VerticalAlignment = VerticalAlignment.Bottom;
            foreach (char item in stack)
            {
                Border elementContainer = new();
                TextBlock elementTextBlock = new();
                elementTextBlock.FontSize = 30;
                elementTextBlock.TextAlignment = TextAlignment.Center;
                elementContainer.Child = elementTextBlock;
                elementContainer.BorderBrush = Brushes.Black;
                elementContainer.Background = Brushes.White;
                elementContainer.BorderThickness = new Thickness(1);
                stackContainer.Children.Add(elementContainer);
                if (stackContainer.Children.Count <= maxInStack)
                {
                    elementTextBlock.Text = item.ToString();
                }
                else
                {
                    elementTextBlock.Text = $"⋮ ({stack.Count - maxInStack})";
                    break;
                }
            }
            border.BorderThickness = new Thickness(2, 0, 2, 2);
            border.BorderBrush = Brushes.Black;
            border.Background = Brushes.LightGray;
            border.Child = stackContainer;
            border.Width = 100;
            border.Height = 300;
            return border;
        }

        public static Border CreateGraphicalTuringMemory(LinkedList<char> memory, int currentIndex)
        {
            string word = string.Concat(memory);
            Border border = new();
            TextBlock container = new();
            Run previous = TextUtils.CreateRunElement(word[..currentIndex], 40, Brushes.Black);
            container.Inlines.Add(previous);
            container.VerticalAlignment = VerticalAlignment.Center;

            if (currentIndex < word.Length)
            {
                Run current = TextUtils.CreateRunElement(word[currentIndex].ToString(), 60, Brushes.LawnGreen);
                Run next = TextUtils.CreateRunElement(word[(currentIndex + 1)..], 40, Brushes.Black);
                current.FontWeight = FontWeights.Bold;
                container.Inlines.Add(current);
                container.Inlines.Add(next);
            }

            border.Child = container;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            border.VerticalAlignment = VerticalAlignment.Center;

            return border;
        }
    }
}

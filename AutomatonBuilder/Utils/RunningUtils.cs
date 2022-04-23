using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Enums;
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
        public static async Task<bool> Run(ModelNode node, IAutomatonMemory memory, Canvas memoryCanvas, int delay = 1000)
        {
            bool movedNode = false;
            TurnOnNode(node);
            memory.PrintMemroy(memoryCanvas);
            await Task.Delay(delay);
            if (memory.IsFinishableWord() && memory.IsLastCharacter())
            {
                node.SetColor(Brushes.White);
                return node.Accepting; //Basic & Pushdown end
            }
            foreach (var connection in node.ConnectedLinesFromThisNode.Keys)
            {
                
                if (connection.ConnectorData.ShouldMove(memory))
                {
                    movedNode = true;
                    IAutomatonMemory clonedMemory = connection.ConnectorData.OnMovementAction(memory);
                    var nextNode = node.ConnectedLinesFromThisNode[connection];
                    node.SetColor(Brushes.White);
                    await Task.Delay(delay / 5);
                    bool solved = await Run(nextNode, clonedMemory, memoryCanvas);

                    if (solved)
                        return true;
                    if (memory.IsDetermenistic())
                        break;
                }
            }

            node.SetColor(Brushes.White);
            return !movedNode && node.Accepting && !memory.IsFinishableWord(); //Turing end
        }

        private static void TurnOnNode(ModelNode node)
        {
            if (node.Accepting)
                node.SetColor(Brushes.LawnGreen);
            else
                node.SetColor(Brushes.Tomato);
        }

        

        public static Border CreateGraphicalBasicMemory(string word, int currentIndex)
        {
            Border border = new();
            TextBlock container = new();
            Run previous = TextUtils.CreateRunElement(word[..currentIndex], 30, Brushes.Gray);
            container.Inlines.Add(previous);

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

        public static Border CreateGraphicalStackMemory(Stack<char> stack)
        {
            Border border = new();
            StackPanel stackContainer = new();
            stackContainer.VerticalAlignment = VerticalAlignment.Bottom;
            foreach (char item in stack)
            {
                Border elementContainer = new();
                TextBlock elementTextBlock = new();
                elementTextBlock.Text = item.ToString();
                elementTextBlock.FontSize = 30;
                elementTextBlock.TextAlignment = TextAlignment.Center;
                elementContainer.Child = elementTextBlock;
                elementContainer.BorderBrush = Brushes.Black;
                elementContainer.BorderThickness = new Thickness(1);
                stackContainer.Children.Add(elementContainer);
            }
            border.BorderThickness = new Thickness(1, 0, 1, 1);
            border.BorderBrush = Brushes.Black;
            border.Child = stackContainer;
            border.Width = 100;
            border.Height = 500;
            return border;
        }


        public static IAutomatonMemory CreateMemory(AutomatonTypes type, string word)
        {
            switch (type)
            {
                case AutomatonTypes.Basic:
                    return new BasicMemory(word);
                case AutomatonTypes.Pushdown:
                    return new StackMemory(word);
                case AutomatonTypes.Turing:
                    return new TuringMemory(word);
                default:
                    throw new Exception("Unsupported type");
            }
        }
    }
}

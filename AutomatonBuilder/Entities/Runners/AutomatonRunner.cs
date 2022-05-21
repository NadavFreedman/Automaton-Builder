using AutomatonBuilder.Entities.AutomatonMemories;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Entities.Exceptions;
using AutomatonBuilder.Entities.Graphics.Memories;
using AutomatonBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.Runners
{
    public class AutomatonRunner
    {
        private readonly GraphicalBasicMemory graphicalBasicMemory;
        private readonly GraphicalStackMemory graphicalStackMemory;
        private readonly GraphicalTuringMemory graphicalTuringMemory;

        private readonly SemaphoreSlim runLock;

        public bool IsAutoRun { get; private set; }
        public bool IsRunning { get; set; }

        public AutomatonRunner()
        {
            this.graphicalBasicMemory = new("", 0, new Point(500, 100));
            this.graphicalStackMemory = new(new Stack<char>(), new Point(1700, 500));
            this.graphicalTuringMemory = new(new LinkedList<char>("├"), 1, new Point(500, 100));
            this.IsAutoRun = true;
            this.runLock = new SemaphoreSlim(1, 1);
        }

        public async Task<bool> Run(ModelNode node, IAutomatonMemory memory, int delay = 1000)
        {
            bool movedNode = false;

            if (!this.IsRunning)
            {
                return false;
            }

            TurnOnNode(node);
            memory.PrintMemroy();

            await this.HandleStep(delay);

            if (memory.IsFinishableWord() && memory.IsLastCharacter())
            {
                node.SetColor(Brushes.White);
                return node.Accepting; //Basic & Pushdown end
            }
            foreach (var connection in node.ConnectorsFromThisNode.ToList())
            {

                if (connection.Value.ConnectorData.ShouldMove(memory))
                {
                    movedNode = true;
                    IAutomatonMemory clonedMemory = connection.Value.ConnectorData.OnMovementAction(memory);
                    var nextNode = connection.Key;
                    node.SetColor(Brushes.White);
                    await Task.Delay(delay / 5);
                    bool solved = await Run(nextNode, clonedMemory, delay);

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

        private async Task HandleStep(int delay)
        {
            if (this.IsAutoRun)
            {
                await Task.Delay(delay);
            }
            if (!this.IsAutoRun)
            {
                await this.runLock.WaitAsync();
                Debug.WriteLine(this.runLock.CurrentCount);
            }
        }


        public IAutomatonMemory CreateMemory(AutomatonTypes type, string word, MainEditingScreen mainWindow)
        {
            switch (type)
            {
                case AutomatonTypes.Basic:
                    this.graphicalBasicMemory.AddToCanvas(mainWindow.MainCanvas);
                    this.graphicalBasicMemory.AllowDragging(mainWindow);
                    this.graphicalBasicMemory.ChangeWord(word, 0);
                    return new BasicMemory(word, graphicalBasicMemory);
                case AutomatonTypes.Pushdown:
                    this.graphicalBasicMemory.AddToCanvas(mainWindow.MainCanvas);
                    this.graphicalBasicMemory.AllowDragging(mainWindow);
                    this.graphicalBasicMemory.ChangeWord(word, 0);
                    this.graphicalStackMemory.AddToCanvas(mainWindow.MainCanvas);
                    this.graphicalStackMemory.AllowDragging(mainWindow);
                    return new StackMemory(word, graphicalBasicMemory, graphicalStackMemory);
                case AutomatonTypes.Turing:
                    this.graphicalTuringMemory.AddToCanvas(mainWindow.MainCanvas);
                    this.graphicalTuringMemory.AllowDragging(mainWindow);
                    this.graphicalTuringMemory.ChangeWord(new LinkedList<char>("├" + word), 1);
                    return new TuringMemory(word, graphicalTuringMemory);
                default:
                    throw new BuilderUnsupportedTypeException("Unsupported type");
            }
        }

        public void Pause()
        {
            this.IsAutoRun = false;
        }

        public void Step()
        {
            if (this.runLock.CurrentCount == 0)
                this.runLock.Release();
        }

        public void Continue()
        {
            this.IsAutoRun = true;
            this.runLock.Release();
        }

        public void Abort()
        {
            this.IsRunning = false;
            if (this.runLock.CurrentCount == 0)
                this.runLock.Release();
        }
    }
}

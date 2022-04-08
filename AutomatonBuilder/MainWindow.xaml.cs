using AutomatonBuilder.Actions.NodeActions;
using AutomatonBuilder.Actions.TextActions;
using AutomatonBuilder.Entities;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Modals;
using Microsoft.Win32;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Actions.MovementActions;
using AutomatonBuilder.Actions.DrawingActions;
using AutomatonBuilder.Utils;

namespace AutomatonBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public readonly AutomatonContext context;

        public MainWindow()
        {
            InitializeComponent();
            this.context = new AutomatonContext(this.MainCanvas);
            this.WindowState = WindowState.Maximized;
        }

        private void AddNodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IAction addNodeAction = new AddNodeAction(this.context, this);
            DoAction(addNodeAction);
        }

        public void RemoveNode(ModelNode node)
        {
            IAction removeNodeAction = new RemoveNodeAction(this.context, this, node);
            DoAction(removeNodeAction);
        }

        public void ConnectNodes(object sender, RoutedEventArgs e)
        {
            ModelNode connectFrom = (ModelNode)((MenuItem)sender).Tag;
            ModelNode connectTo = ConnectorUtils.GetNodeByName(this.context, ((MenuItem)sender).Header.ToString()!);

            ConnectorInput connectorInputWindow = new(connectFrom.ToString(), connectTo.ToString());
            connectorInputWindow.ShowDialog();
            string input;
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.Input;
            else
                return;

            IAction connectAction = new ConnectNodesAction(this.context, connectFrom, connectTo, input, this);
            DoAction(connectAction);
        }

        public void RemoveConnector_Click(object sender, RoutedEventArgs e)
        {
            IConnector connector = (IConnector)((MenuItem)sender).Tag;
            IAction disconnectAction = new DisconnectNodesAction(this.context, connector);
            DoAction(disconnectAction);
        }

        public void RemoveText_Click(object sender, RoutedEventArgs e)
        {
            BorderedText text = (BorderedText)((MenuItem)sender).Tag;
            IAction removeText = new RemoveTextAction(this.context, text);
            DoAction(removeText);

        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            this.context.MouseProperies.WasLeftMouseKeyPressedLastTick = this.context.MouseProperies.IsLeftMouseKeyPressed;
            this.context.MouseProperies.IsLeftMouseKeyPressed = e.LeftButton == MouseButtonState.Pressed;

            if (this.context.MouseProperies.IsLeftMouseKeyPressed)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (this.context.CurrentLine.Points.Count != 0)
                    {
                        IAction drawLineAction = new DrawLineAction(this.context);
                        DoAction(drawLineAction);
                    }

                    if (Mouse.DirectlyOver is Polyline line)
                    {
                        IAction deleteLineAction = new DeleteLineAction(this.context, line);
                        DoAction(deleteLineAction);
                    }
                }
                else if (this.context.MouseProperies.HoveredElement is null)
                {
                    if (!this.context.MouseProperies.WasLeftMouseKeyPressedLastTick)
                        this.context.MouseProperies.LeftClickHoldStartingPosition = e.GetPosition(this);

                    this.context.CurrentLine.Points.Add(e.GetPosition(this));
                }
                else
                {
                    if (!this.context.MouseProperies.WasLeftMouseKeyPressedLastTick)
                        this.context.MouseProperies.LeftClickHoldStartingPosition = this.context.MouseProperies.HoveredElement.GetPosition();

                    this.context.MouseProperies.HoveredElement.SetPosition(e.GetPosition(this));
                }
                
            }
            else
            {
                if (this.context.MouseProperies.WasLeftMouseKeyPressedLastTick)
                {
                    this.context.MouseProperies.LeftClickHoldReleasePosition = e.GetPosition(this);

                    if (this.context.CurrentLine!.Points.Count != 0)
                    {
                        IAction drawingAction = new DrawLineAction(context);
                        DoAction(drawingAction);
                    }
                    else if (this.context.MouseProperies.HoveredElement is not null)
                    {
                        IAction moveTextAction = new MoveElementAction(this.context.MouseProperies.HoveredElement, 
                            this.context.MouseProperies.LeftClickHoldStartingPosition, 
                            this.context.MouseProperies.LeftClickHoldReleasePosition);
                        DoAction(moveTextAction);
                    }
                }
            }

        }

        public void GeneralElement_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.context.MouseProperies.HoveredElement is not null) return;
            if (this.context.MouseProperies.WasLeftMouseKeyPressedLastTick) return;
            IMoveable hovered = (IMoveable)sender;
            this.context.MouseProperies.HoveredElement = hovered;
        }

        public void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.context.MouseProperies.IsLeftMouseKeyPressed)
                this.context.MouseProperies.HoveredElement = null;
        }

        public void TaggedElement_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.context.MouseProperies.HoveredElement is not null) return;
            if (this.context.MouseProperies.WasLeftMouseKeyPressedLastTick) return;
            this.context.MouseProperies.HoveredElement = (IMoveable)((FrameworkElement)sender).Tag;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //If the left mouse key is pressed then don't do anything
            if (!this.context.MouseProperies.IsLeftMouseKeyPressed)
            {
                //If the user holds Ctrl
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (e.Key == Key.Z && this.context.DoneActionsStack.Count != 0) //Undo
                        UndoBtn_Click(null, null);
                    else if (e.Key == Key.Y && this.context.UndoneActionsStack.Count != 0) //Redo
                        RedoBtn_Click(null, null);
                    else if (e.Key == Key.S) //Save
                        SaveBtn_Click(null, null);
                    else if (e.Key == Key.T) //Add text
                    {
                        this.context.MouseProperies.LastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
                        AddTextMenuItem_Click(null, null);
                    }
                    else if (e.Key == Key.N) //Add node
                    {
                        this.context.MouseProperies.LastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
                        AddNodeMenuItem_Click(null, null);
                    }
                }
            }
        }


        private void AddTextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Open an input dialog
            ConnectorInput connectorInputWindow = new ConnectorInput("Add Text");
            connectorInputWindow.ShowDialog();
            string input;
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.Input;
            else
                return;

            IAction addTextAction = new AddTextAction(this.context, input, this);
            DoAction(addTextAction);
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.context.MouseProperies.LastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Open a save dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Images (.png)|*.png|Editable Automaton File (.aut)|*.aut",
            };



            //Save the file
            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                string extension = System.IO.Path.GetExtension(saveFileDialog.FileName);

                switch (extension)
                {
                    case ".png":
                        SavingUtils.SaveAsPng(this, saveFileDialog.FileName);
                        break;
                    case ".aut":
                        SavingUtils.SaveAsAut(this, saveFileDialog.FileName);
                        break;
                    default:
                        throw new Exception("Invalid file format");
                }
            }

            //Show the tool bar
            this.MainToolBar.Visibility = Visibility.Visible;
        }

        private void UndoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.context.DoneActionsStack.Count != 0)
            {
                IAction actionToUndo = this.context.DoneActionsStack.Pop();
                actionToUndo.UndoAction();
                this.context.UndoneActionsStack.Push(actionToUndo);
                this.RedoBtn.IsEnabled = true;
            }
            if (this.context.DoneActionsStack.Count == 0)
            {
                this.UndoBtn.IsEnabled = false;
            }
        }

        private void RedoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.context.UndoneActionsStack.Count != 0)
            {
                IAction actionToRedo = this.context.UndoneActionsStack.Pop();
                actionToRedo.RedoAction();
                this.context.DoneActionsStack.Push(actionToRedo);
                this.UndoBtn.IsEnabled = true;
            }
            if (this.context.UndoneActionsStack.Count == 0)
            {
                this.RedoBtn.IsEnabled = false;
            }
        }

        private void DoAction(IAction action)
        {
            action.DoAction();
            this.context.DoneActionsStack.Push(action);
            this.context.UndoneActionsStack.Clear();
            this.RedoBtn.IsEnabled = false;
            this.UndoBtn.IsEnabled = true;
        }
    }
}

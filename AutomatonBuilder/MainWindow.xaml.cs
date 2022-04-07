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

        /// <summary>
        /// Handles "Add Node" Click or Ctrl+N
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void AddNodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IAction addNodeAction = new AddNodeAction(this.context, this);
            DoAction(addNodeAction);
        }

        /// <summary>
        /// Handles removal of a node.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        public void RemoveNode(ModelNode node)
        {
            IAction removeNodeAction = new RemoveNodeAction(this.context, this, node);
            DoAction(removeNodeAction);
        }


        /// <summary>
        /// Connect the sender node to another node
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        public void ConnectNodes(object sender, RoutedEventArgs e)
        {
            //Get the node that is associated to the MenuItem
            ModelNode connectFrom = (ModelNode)((MenuItem)sender).Tag;
            ModelNode connectTo = ConnectorUtils.GetNodeByName(this.context, ((MenuItem)sender).Header.ToString());


            //Get the Text to be written on the arrow
            ConnectorInput connectorInputWindow = new ConnectorInput(connectFrom.ToString(), connectTo.ToString());
            connectorInputWindow.ShowDialog();
            string input;
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.Input;
            else
                return;

            IAction connectAction = new ConnectNodesAction(this.context, connectFrom, connectTo, input, this);
            DoAction(connectAction);
        }

        /// <summary>
        /// Handles "Remove" click
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
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

        /// <summary>
        /// Handles mouse movement
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            this.context.WasLeftMouseKeyPressedLastTick = this.context.IsLeftMouseKeyPressed;
            this.context.IsLeftMouseKeyPressed = e.LeftButton == MouseButtonState.Pressed;

            if (Keyboard.IsKeyDown(Key.LeftShift)) { }
            else if (this.context.IsLeftMouseKeyPressed)
            {
                if (!this.context.WasLeftMouseKeyPressedLastTick)
                    this.context.LeftClickHoldStartingPosition = e.GetPosition(this);

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (this.context.CurrentLine.Points.Count != 0)
                    {
                        IAction drawingAction = new DrawLineAction(this.context);
                        DoAction(drawingAction);
                    }

                    if (Mouse.DirectlyOver is Polyline line)
                    {
                        IAction deleteLineAction = new DeleteLineAction(this.context, line);
                        DoAction(deleteLineAction);
                    }
                }
                else if (this.context.HoveredElement is null)
                {
                    this.context.CurrentLine.Points.Add(e.GetPosition(this));
                }
                else
                {
                    this.context.HoveredElement.SetPosition(e.GetPosition(this));
                }
                
            }
            else
            {
                if (this.context.WasLeftMouseKeyPressedLastTick)
                {
                    this.context.LeftClickHoldReleasePosition = e.GetPosition(this);

                    if (this.context.CurrentLine!.Points.Count != 0)
                    {
                        IAction drawingAction = new DrawLineAction(context);
                        DoAction(drawingAction);
                    }
                    else if (this.context.HoveredElement is not null)
                    {
                        IAction moveTextAction = new MoveElementAction(this.context.HoveredElement, this.context.LeftClickHoldStartingPosition, this.context.LeftClickHoldReleasePosition);
                        DoAction(moveTextAction);
                    }
                }
            }

        }

        public void GeneralElement_MouseEnter(object sender, MouseEventArgs e)
        {
            IMoveable hovered = (IMoveable)sender;
            this.context.HoveredElement = hovered;
        }

        public void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.context.IsLeftMouseKeyPressed)
                this.context.HoveredElement = null;
        }

        public void TaggedElement_MouseEnter(object sender, MouseEventArgs e)
        {
            this.context.HoveredElement = (IMoveable)((FrameworkElement)sender).Tag;
        }


        /// <summary>
        /// Handles the pressing of the key.
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //If the left mouse key is pressed then don't do anything
            if (!this.context.IsLeftMouseKeyPressed)
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
                        this.context.LastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
                        AddTextMenuItem_Click(null, null);
                    }
                    else if (e.Key == Key.N) //Add node
                    {
                        this.context.LastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
                        AddNodeMenuItem_Click(null, null);
                    }
                }
            }
        }


        /// <summary>
        /// Handles the "Add Text".
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void AddTextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Open an input dialog
            ConnectorInput connectorInputWindow = new ConnectorInput("Add Text");
            connectorInputWindow.ShowDialog();
            string input = "";
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.Input;
            else
                return;

            IAction addTextAction = new AddTextAction(this.context, input, this);
            DoAction(addTextAction);
        }

        /// <summary>
        /// Saves the current position of the mouse to a field.
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.context.LastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
        }


        /// <summary>
        /// Handles Save Click
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Hide the tool bar
            this.MainToolBar.Visibility = Visibility.Hidden;

            //Save the screen to bitmap
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.MainCanvas.ActualWidth, (int)this.MainCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(this.MainCanvas);
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            MemoryStream stream = new MemoryStream();
            png.Save(stream);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

            //Open a save dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Filter = "Images|*.png",
            };

            //Default format
            ImageFormat format = ImageFormat.Png;

            //Save the file
            if (saveFileDialog.ShowDialog() == DialogResult)
            {
                image.Save(saveFileDialog.FileName, format);
                MessageBox.Show("The model has been saved successfully.", "Copied successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //Show the tool bar
            this.MainToolBar.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Handles "Undo" Click
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
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

        /// <summary>
        /// Handles "Redo" Click
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
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

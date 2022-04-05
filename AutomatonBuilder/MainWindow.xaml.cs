using AutomatonBuilder.Actions;
using AutomatonBuilder.Actions.NodeActions;
using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Actions;
using AutomatonBuilder.Entities.Actions.TextActions;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Modals;
using AutomatonBuilder.Utils;
using Microsoft.Win32;
using Petzold.Media2D;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            string connectToName = ((MenuItem)sender).Header.ToString();
            ModelNode connectTo = null;

            //Get the node that is associated to the MenuItem
            ModelNode connectFrom = (ModelNode)((MenuItem)sender).Tag;

            //Get the destination node.
            foreach (ModelNode node in this.context.NodesList)
                if (node.ToString() == connectToName)
                {
                    connectTo = node;
                    break;
                }

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
            Border box = (Border)((MenuItem)sender).Tag;
            IAction removeText = new RemoveTextAction(this.context, box);
            DoAction(removeText);

        }

        /// <summary>
        /// Handles mouse movement
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //If the left mouse button is pressed
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Turn on the mouse flag.
                this.context.IsLeftMouseKeyPressed = true;

                if (this.context.HoveredElement == null)
                {
                    //Create the ellipse
                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = Brushes.Black,
                        Width = 6,
                        Height = 6,
                    };

                    //Set the coordinates of the ellipse.
                    Canvas.SetLeft(ellipse, e.GetPosition(this).X - 3);
                    Canvas.SetTop(ellipse, e.GetPosition(this).Y - 3);

                    //If Ctrl is held then it's removal
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ellipse.Fill = Brushes.White;
                        ellipse.Width = 20;
                        ellipse.Height = 20;
                        Canvas.SetLeft(ellipse, e.GetPosition(this).X - 10);
                        Canvas.SetTop(ellipse, e.GetPosition(this).Y - 10);
                    }

                    //Add the ellipse to the currently drawn line and print it to the screen.
                    this.context.CurrentLine.Add(ellipse);
                    MainCanvas.Children.Add(ellipse);
                }
                else
                {
                    if (this.context.HoveredElement is ModelNode node)
                    {
                        node.SetPosition(e.GetPosition(this));
                    }
                }
                
            }
            else
            {
                //Turn off the mouse flag.
                this.context.IsLeftMouseKeyPressed = false;

                //If there was a line currently drawn, save to the Undo stack and create a new, empty line.
                if (this.context.CurrentLine.Count != 0)
                {
                    IAction drawingAction = new DrawLineAction(context);
                    context.CurrentLine = new List<Ellipse>();
                    DoAction(drawingAction);

                }
            }
        }

        public void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement hovered = (UIElement)sender;
            this.context.HoveredElement = hovered;
            Title = $"{this.context.HoveredElement}";
        }

        public void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            this.context.HoveredElement = null;
            Title = $"{this.context.HoveredElement}";
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

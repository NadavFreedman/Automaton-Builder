﻿using AutomatonBuilder.Entities;
using AutomatonBuilder.Modals;
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
        /// <summary>
        /// The number of nodes currently on the screen.
        /// </summary>
        private int nodeCount;

        /// <summary>
        /// A list containing all of the names of the nodes currently on the screen
        /// </summary>
        private readonly List<ModelNode> nodesList;

        /// <summary>
        /// A stack storing the lines that can be undone
        /// </summary>
        private readonly Stack<List<Ellipse>> UndoStack;

        /// <summary>
        /// A stack storing the lines that can be redone
        /// </summary>
        private readonly Stack<List<Ellipse>> RedoStack;

        /// <summary>
        /// A list containing the ellipses that are part of the current line drawn
        /// </summary>
        private List<Ellipse> currentLine;

        /// <summary>
        /// A flag indicating whether the left mouse button is currently pressed.
        /// </summary>
        private bool IsLeftMouseKeyPressed;

        /// <summary>
        /// The position in which the right click was pressed last time
        /// </summary>
        private Point lastRightClickPosition;

        /// <summary>
        /// The starting node
        /// </summary>
        private ModelNode startingNode;

        public MainWindow()
        {
            InitializeComponent();
            //Initialize the fields
            this.nodeCount = 0;
            this.nodesList = new List<ModelNode>();
            this.UndoStack = new Stack<List<Ellipse>>();
            this.RedoStack = new Stack<List<Ellipse>>();
            this.currentLine = new List<Ellipse>();
            this.lastRightClickPosition = new Point();
            this.IsLeftMouseKeyPressed = false;
            this.startingNode = null;
        }

        /// <summary>
        /// Handles "Add Node" Click or Ctrl+N
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void AddNodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Create the new node
            ModelNode node = new ModelNode(this.nodeCount++, this, this.lastRightClickPosition);

            //Auto-set the node to the starting node if it's the only one
            if (this.nodeCount == 1)
                ToggleNodeStarting(node);

            //Add the node to the Canvas
            Canvas.SetLeft(node, lastRightClickPosition.X - node.Size / 2);
            Canvas.SetTop(node, lastRightClickPosition.Y - node.Size / 2);
            this.MainCanvas.Children.Add(node);
            this.nodesList.Add(node);

            //Add the node to each of the context menus of the existing nodes
            foreach (var item in this.MainCanvas.Children)
                if (item is ModelNode existingNode)
                {
                    existingNode.ConnectMenuItem.Items.Clear();
                    foreach (ModelNode q in this.nodesList)
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Click += ConnectNodes;
                        //Link the node to the MenuItem
                        menuItem.Tag = existingNode;
                        menuItem.Header = q.ToString();
                        existingNode.ConnectMenuItem.Items.Add(menuItem);
                    }
                }



        }

        /// <summary>
        /// Handles removal of a node.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        public void RemoveNode(ModelNode node)
        {
            //Decrease the index of each node that was created after the current node.
            for (int i = this.MainCanvas.Children.Count - 1; i >= 0; i--)
            {
                object item = this.MainCanvas.Children[i];
                if (item is ModelNode itemNode)
                {
                    if (itemNode == node)
                        break;
                    else
                        itemNode.Index--;
                }
            }

            //Remove each line connected to the node.
            foreach (object connector in node.connectedLines)
            {
                MainCanvas.Children.Remove((UIElement)connector);
                MainCanvas.Children.Remove((UIElement)((Shape)connector).Tag);
            }

            //if the node was the starting node, set it to null and remove the arrow from the screen.
            if (node.Starting)
            {
                this.startingNode = null;
                this.MainCanvas.Children.Remove(node.startingArrow);
            }

            //Remove the node itself and decrease the amount of nodes on screen.
            this.MainCanvas.Children.Remove(node);
            this.nodesList.Remove(this.nodesList[this.nodesList.Count - 1]);
            this.nodeCount--;

            //Remove the node from the context menu of each existing node.
            foreach (ModelNode existingNode in this.nodesList)
            {
                existingNode.ConnectMenuItem.Items.Clear();
                foreach (ModelNode q in this.nodesList)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Click += ConnectNodes;
                    menuItem.Tag = existingNode;
                    menuItem.Header = q.ToString();
                    existingNode.ConnectMenuItem.Items.Add(menuItem);
                }
            }
        }

        /// <summary>
        /// Toggle the node to be the starting node if it is, or vice versa.
        /// </summary>
        /// <param name="node">The node to be toggled.</param>
        public void ToggleNodeStarting(ModelNode node)
        {
            //Toggle the starting flag.
            node.Starting = !node.Starting;

            if (node.Starting)
            {
                //Add the starting arrow to the canvas
                this.MainCanvas.Children.Add(node.startingArrow);

                //if there was a starting node, toggle it.
                if (this.startingNode != null)
                    ToggleNodeStarting(this.startingNode);

                //Set this node to be the starting node
                this.startingNode = node;
            }
            else //Remove the starting node of the used-to-be-starting node
                this.MainCanvas.Children.Remove(node.startingArrow);
        }

        /// <summary>
        /// Connect the sender node to another node
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void ConnectNodes(object sender, RoutedEventArgs e)
        {
            string connectToName = ((MenuItem)sender).Header.ToString();
            ModelNode connectTo = null;

            //Get the node that is associated to the MenuItem
            ModelNode connectFrom = (ModelNode)((MenuItem)sender).Tag;

            //Get the destination node.
            foreach (ModelNode node in this.nodesList)
                if (node.ToString() == connectToName)
                {
                    connectTo = node;
                    break;
                }

            //The position of the Text Block
            Point lineTextPoint = new Point();

            //Get the Text to be written on the arrow
            ConnectorInput connectorInputWindow = new ConnectorInput(connectFrom.ToString(), connectTo.ToString());
            connectorInputWindow.ShowDialog();
            string input = "";
            if (connectorInputWindow.DialogResult == true)
                input = connectorInputWindow.Input;
            else
                return;

            //Build the Text Block
            TextBlock lineTextBlock = new TextBlock
            {
                Background = Brushes.White,
                Text = input,
                Margin = new Thickness(3)
            };

            //Build the border of the block
            Border border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White,
                Child = lineTextBlock
            };

            //Calculate the size of the block roughly.
            var formattedText = new FormattedText(
                                        input,
                                        CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface(lineTextBlock.FontFamily, lineTextBlock.FontStyle, lineTextBlock.FontWeight, lineTextBlock.FontStretch),
                                        lineTextBlock.FontSize,
                                        Brushes.Black,
                                        new NumberSubstitution(),
                                        1);


            //If the node needs to be connected to itself
            if (connectFrom.Position.X == connectTo.Position.X && connectTo.Position.Y == connectFrom.Position.Y)
            {
                //Create the ellipse and add it to the canvas. 
                Ellipse connectorEllipse = new Ellipse
                {
                    Width = 50,
                    Height = 50,
                    Stroke = Brushes.Black,
                    StrokeThickness = 3,
                    Visibility = Visibility.Visible
                };
                Canvas.SetLeft(connectorEllipse, connectFrom.Position.X - 45);
                Canvas.SetTop(connectorEllipse, connectFrom.Position.Y - 45);
                connectFrom.connectedLines.Add(connectorEllipse);

                //Add a context menu to the ellipse
                MenuItem removeConnector = new MenuItem
                {
                    Header = "Remove",
                    Tag = connectorEllipse
                };
                removeConnector.Click += RemoveConnector_Click;
                connectorEllipse.ContextMenu = new ContextMenu();
                connectorEllipse.ContextMenu.Items.Add(removeConnector);

                //Add a context menu to the text block
                MenuItem removeConnector1 = new MenuItem
                {
                    Header = "Remove",
                    Tag = border
                };
                removeConnector1.Click += RemoveConnector_Click;
                border.ContextMenu = new ContextMenu();
                border.ContextMenu.Items.Add(removeConnector1);

                //Calculate the location of the block
                lineTextPoint = new Point(connectFrom.Position.X - 45, connectFrom.Position.Y - 45);

                //Insert the line at the bottom of the canvas (Z-axis wise)
                MainCanvas.Children.Insert(0, connectorEllipse);

                //Connect the border to the ellipse for future removal
                connectorEllipse.Tag = border;
            }
            else
            {
                //Calculate the positions of the arrow.
                double deltaX = connectTo.Position.X - connectFrom.Position.X;
                double deltaY = connectTo.Position.Y - connectFrom.Position.Y;
                double alpha = Math.Atan(deltaY / deltaX);
                if (deltaX < 0 && deltaY < 0 || deltaY > 0 && deltaX < 0)
                    alpha -= Math.PI;
                ArrowLine connector = new ArrowLine
                {
                    Visibility = Visibility.Visible,
                    StrokeThickness = 3,
                    Stroke = Brushes.Black,

                    X1 = connectFrom.Position.X,
                    Y1 = connectFrom.Position.Y,

                    X2 = connectTo.Position.X - Math.Cos(alpha) * (connectTo.Size / 2),
                    Y2 = connectTo.Position.Y - Math.Sin(alpha) * (connectTo.Size / 2)
                };



                //Add a context menu to the arrow
                MenuItem removeConnector1 = new MenuItem
                {
                    Header = "Remove",
                    Tag = connector
                };
                removeConnector1.Click += RemoveConnector_Click;
                connector.ContextMenu = new ContextMenu();
                connector.ContextMenu.Items.Add(removeConnector1);

                //Add a context menu to the text block
                MenuItem removeConnector = new MenuItem
                {
                    Header = "Remove",
                    Tag = border
                };
                removeConnector.Click += RemoveConnector_Click;
                border.ContextMenu = new ContextMenu();
                border.ContextMenu.Items.Add(removeConnector);

                //Add the line to both of the nodes for future removal
                connectFrom.connectedLines.Add(connector);
                connectTo.connectedLines.Add(connector);

                //Insert the line at the bottom of the Canvas
                this.MainCanvas.Children.Insert(0, connector);

                //calculate the middle point of the line
                lineTextPoint = new Point(connectFrom.Position.X + deltaX / 2, connectFrom.Position.Y + deltaY / 2);

                //Connect the line to the border for future removal
                connector.Tag = border;
            }

            //Add the text to the canvas
            Canvas.SetLeft(border, lineTextPoint.X - 2 - formattedText.Width / 2);
            Canvas.SetTop(border, lineTextPoint.Y - 2 - formattedText.Height / 2);
            this.MainCanvas.Children.Add(border);
        }

        /// <summary>
        /// Handles "Remove" click
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void RemoveConnector_Click(object sender, RoutedEventArgs e)
        {
            UIElement connector = (UIElement)((MenuItem)sender).Tag;
            this.MainCanvas.Children.Remove(connector);
            if (connector is Shape s)
                if (s.Tag != null)
                    MainCanvas.Children.Remove((UIElement)s.Tag);
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
                this.IsLeftMouseKeyPressed = true;

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
                this.currentLine.Add(ellipse);
                MainCanvas.Children.Add(ellipse);
            }
            else
            {
                //Turn off the mouse flag.
                this.IsLeftMouseKeyPressed = false;

                //If there was a line currently drawn, save to the Undo stack and create a new, empty line.
                if (this.currentLine.Count != 0)
                {
                    this.RedoStack.Clear();
                    this.UndoStack.Push(this.currentLine);
                    this.currentLine = new List<Ellipse>();
                }
            }
        }


        /// <summary>
        /// Handles the pressing of the key.
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //If the left mouse key is pressed then don't do anything
            if (!this.IsLeftMouseKeyPressed)
            {
                //If the user holds Ctrl
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (e.Key == Key.Z && this.UndoStack.Count != 0) //Undo
                        UndoBtn_Click(null, null);
                    else if (e.Key == Key.Y && this.RedoStack.Count != 0) //Redo
                        RedoBtn_Click(null, null);
                    else if (e.Key == Key.S) //Save
                        SaveBtn_Click(null, null);
                    else if (e.Key == Key.T) //Add text
                    {
                        this.lastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
                        AddTextMenuItem_Click(null, null);
                    }
                    else if (e.Key == Key.N) //Add node
                    {
                        this.lastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
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

            //Create a text block
            TextBlock lineTextBlock = new TextBlock
            {
                Background = Brushes.White,
                Text = input,
                Margin = new Thickness(3)
            };

            //Create a border
            Border border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White,
                Child = lineTextBlock
            };

            //Calculate the size of the text block
            var formattedText = new FormattedText(
                                        input,
                                        CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface(lineTextBlock.FontFamily, lineTextBlock.FontStyle, lineTextBlock.FontWeight, lineTextBlock.FontStretch),
                                        lineTextBlock.FontSize,
                                        Brushes.Black,
                                        new NumberSubstitution(),
                                        1);

            //Add a context menu to the text block
            border.ContextMenu = new ContextMenu();
            MenuItem removeTextItem = new MenuItem
            {
                Header = "Remove",
                Tag = border
            };
            removeTextItem.Click += RemoveConnector_Click;
            border.ContextMenu.Items.Add(removeTextItem);

            //Add the block to the canvas
            Canvas.SetLeft(border, lastRightClickPosition.X - 2 - formattedText.Width / 2);
            Canvas.SetTop(border, lastRightClickPosition.Y - 2 - formattedText.Height / 2);
            this.MainCanvas.Children.Add(border);
        }

        /// <summary>
        /// Saves the current position of the mouse to a field.
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.lastRightClickPosition = Mouse.GetPosition(this.MainCanvas);
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
            if (this.UndoStack.Count != 0)
            {
                //Remove the line 
                List<Ellipse> linesToRemove = this.UndoStack.Pop();
                this.RedoStack.Push(linesToRemove);
                foreach (Ellipse lineToRemove in linesToRemove)
                    this.MainCanvas.Children.Remove(lineToRemove);
            }
        }

        /// <summary>
        /// Handles "Redo" Click
        /// </summary>
        /// <param name="sender">The object which fired this event</param>
        /// <param name="e">Event Arguments</param>
        private void RedoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.RedoStack.Count != 0)
            {
                //Redo the line
                List<Ellipse> linesToAdd = this.RedoStack.Pop();
                this.UndoStack.Push(linesToAdd);
                foreach (Ellipse lineToRemove in linesToAdd)
                    this.MainCanvas.Children.Add(lineToRemove);
            }
        }
    }
}

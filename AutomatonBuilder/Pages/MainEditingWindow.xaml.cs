using AutomatonBuilder.Actions.NodeActions;
using AutomatonBuilder.Actions.TextActions;
using AutomatonBuilder.Entities;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Modals.ConnectionModals;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Actions.MovementActions;
using AutomatonBuilder.Actions.DrawingActions;
using AutomatonBuilder.Utils;
using AutomatonBuilder.Entities.Contexts;
using AutomatonBuilder.Entities.Runners;
using AutomatonBuilder.Entities.Args;
using AutomatonBuilder.Entities.Enums;
using AutomatonBuilder.Modals;
using AutomatonBuilder.Entities.AutomatonMemories;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AutomatonBuilder.Entities.Exceptions;

namespace AutomatonBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainEditingScreen : Page
    {

        public AutomatonContext context;
        private readonly AutomatonRunner runner;
        private readonly MainWindow host;

        public MainEditingScreen(MainWindow host, AutomatonTypes type = AutomatonTypes.Basic)
        {
            InitializeComponent();
            this.context = new(this.MainCanvas, this, type);
            this.host = host;
            this.runner = new();
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

            if (connectFrom.ConnectorsFromThisNode.ContainsKey(connectTo))
            {
                IAction connectAction = new EditConnectionAction(this.context, connectFrom, connectTo);
                DoAction(connectAction);
            }
            else
            {
                IAction connectAction = new ConnectNodesAction(this.context, connectFrom, connectTo, this);
                DoAction(connectAction);
            }
            
        }

        public void RemoveConnector_Click(object sender, RoutedEventArgs e)
        {
            IConnector connector = (IConnector)((MenuItem)sender).Tag;
            IAction disconnectAction = new DisconnectNodesAction(this.context, connector);
            DoAction(disconnectAction);
        }

        public void EditConnector_Click(object sender, RoutedEventArgs e)
        {
            IConnector connector = (IConnector)((MenuItem)sender).Tag;
            IAction editConnectionAction = new EditConnectionAction(this.context, connector);
            DoAction(editConnectionAction);
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

            if (this.ColorPicker.IsOpen) return;

            if (this.context.MouseProperies.IsLeftMouseKeyPressed)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (this.context.CurrentLine.Points.Count != 0)
                    {
                        IAction drawLineAction = new DrawLineAction(this.context, this);
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
                        IAction drawingAction = new DrawLineAction(context, this);
                        DoAction(drawingAction);
                    }
                    else if (this.context.MouseProperies.HoveredElement is not null)
                    {
                        IAction moveTextAction = new MoveElementAction(this.context.MouseProperies.HoveredElement, 
                            this.context.MouseProperies.LeftClickHoldStartingPosition, 
                            this.context.MouseProperies.LeftClickHoldReleasePosition);
                        DoAction(moveTextAction);
                    }

                    this.context.MouseProperies.HoveredElement = null;
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
            if (!this.context.MouseProperies.IsLeftMouseKeyPressed)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (e.Key == Key.Z && this.context.DoneActionsStack.Count != 0)
                        UndoBtn_Click(null, null);
                    else if (e.Key == Key.Y && this.context.UndoneActionsStack.Count != 0)
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
                    else if (e.Key == Key.OemPlus)
                    {
                        this.FontSizeComboBox.SelectedIndex = Math.Min(this.FontSizeComboBox.SelectedIndex + 1, this.FontSizeComboBox.Items.Count);
                    }
                    else if (e.Key == Key.OemMinus)
                    {
                        this.FontSizeComboBox.SelectedIndex = Math.Max(this.FontSizeComboBox.SelectedIndex - 1, 0);
                    }
                }
                else if (e.Key == Key.F5 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    AbortBtn_Click(null, null);
                }
                else if (e.Key == Key.F5)
                {
                    PlayBtn_ClickAsync(null, null);
                }
                else if (e.Key == Key.F9)
                {
                    TogglePausePlayBtn_Click(null, null);
                }
                else if (Keyboard.IsKeyDown(Key.F10))
                {
                    StepBtn_Click(null, null);
                }
            }
        }


        private void AddTextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Open an input dialog
            TextInputModal textInputWindow = new();
            textInputWindow.ShowDialog();
            AddTextArgs input;
            if (textInputWindow.DialogResult == true)
                input = textInputWindow.Input;
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
                Filter = "Editable Automaton Files (.eaf)|*.eaf|Images (.png)|*.png",
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
                    case ".eaf":
                        SavingUtils.SaveAsEaf(this, saveFileDialog.FileName);
                        break;
                    default:
                        throw new BuilderInvalidActionException("Tried to save in an invalid file format");
                }
            }


        }

        private void UndoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.context.DoneActionsStack.Count != 0)
            {
                IAction actionToUndo = this.context.DoneActionsStack.Pop();
                actionToUndo.UndoAction();
                this.context.UndoneActionsStack.Push(actionToUndo);
                this.RedoBtn.IsEnabled = true;
                if (this.context.DoneActionsStack.Count == 0)
                    this.ChangeTitle(this.host.Title[0..(this.host.Title.Length - 1)]);
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
                if (this.context.DoneActionsStack.Count == 0)
                    this.ChangeTitle(this.host.Title + "*");
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
            if (action.CanceledAction) return;
            this.context.DoneActionsStack.Push(action);
            this.context.UndoneActionsStack.Clear();
            this.RedoBtn.IsEnabled = false;
            this.UndoBtn.IsEnabled = true;
            if (!this.host.Title.EndsWith("*"))
                this.host.Title += "*";
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (this.context?.CurrentLine is null) return;

            this.context.CurrentLine.Stroke = new SolidColorBrush(e.NewValue!.Value);
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.context?.CurrentLine is null) return;

            this.context.CurrentLine.StrokeThickness = double.Parse(((ComboBoxItem)FontSizeComboBox.SelectedValue).Content.ToString()!);
        }

        private void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainCanvas.Focus();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.context.DoneActionsStack.Count != 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("You have unsaved changes. Are you sure you want to exit?", "Exit Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                    return;
            }
            this.host.GoToMenu();
        }

        private async void PlayBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (this.runner.IsRunning) return;

            RunWordModal wordModal = new();
            if (wordModal.ShowDialog() == false) return;

            this.runner.IsRunning = true;
            this.EanableButtons();

            bool result = await runner.Run(this.context.StartingNode!, runner.CreateMemory(this.context.type, wordModal.EnteredWord, this), wordModal.DelayInMilliseconds);
            if (result)
                MessageBox.Show($"The word was accepted!");
            else
                MessageBox.Show($"The word wasn't accepted!");

            this.runner.IsRunning = false;
            this.EanableButtons();
        }

        private void StepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.runner.IsRunning && !this.runner.IsAutoRun)
                this.runner.Step();
        }

        private void TogglePausePlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!this.runner.IsRunning) return;
            if (this.runner.IsAutoRun)
            {
                this.runner.Pause();
                this.ToggleAutoRunBtn.ToolTip = "Continue";
            }
            else
            {
                this.runner.Continue();
                this.ToggleAutoRunBtn.ToolTip = "Pause";
            }
            this.StepBtn.IsEnabled = !this.runner.IsAutoRun;
        }

        private void AbortBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.runner.IsRunning)
                this.runner.Abort();
        }

        private void EanableButtons()
        {
            this.PlayBtn.IsEnabled = !this.runner.IsRunning;
            this.ToggleAutoRunBtn.IsEnabled = this.runner.IsRunning;
            this.StepBtn.IsEnabled = !this.runner.IsAutoRun && this.runner.IsRunning;
            this.AbortBtn.IsEnabled = this.runner.IsRunning;
        }

        public void ChangeTitle(string newTitle)
        {
            this.host.Title = newTitle;
        }
    }
}

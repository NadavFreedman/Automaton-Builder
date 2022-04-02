using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AutomatonBuilder.Entities
{
    public class AutomatonContext
    {
        /// <summary>
        /// A list containing all of the nodes currently on the screen
        /// </summary>
        public readonly List<ModelNode> NodesList;

        /// <summary>
        /// A stack storing the lines that can be undone
        /// </summary>
        public readonly Stack<IAction> DoneActionsStack;

        /// <summary>
        /// A stack storing the lines that can be redone
        /// </summary>
        public readonly Stack<IAction> UndoneActionsStack;

        /// <summary>
        /// A list containing the ellipses that are part of the current line drawn
        /// </summary>
        public List<Ellipse>? CurrentLine;

        /// <summary>
        /// A flag indicating whether the left mouse button is currently pressed.
        /// </summary>
        public bool IsLeftMouseKeyPressed;

        /// <summary>
        /// The position in which the right click was pressed last time
        /// </summary>
        public Point LastRightClickPosition;

        /// <summary>
        /// The starting node
        /// </summary>
        public ModelNode? StartingNode;

        public Canvas MainCanvas;



        public AutomatonContext(Canvas windowCanvas)
        {
            this.StartingNode = null;
            this.NodesList = new List<ModelNode>();
            this.UndoneActionsStack = new Stack<IAction>();
            this.DoneActionsStack = new Stack<IAction>();
            this.CurrentLine = new List<Ellipse>();
            this.LastRightClickPosition = new Point();
            this.IsLeftMouseKeyPressed = false;
            this.StartingNode = null;
            this.MainCanvas = windowCanvas;
        }
    }
}

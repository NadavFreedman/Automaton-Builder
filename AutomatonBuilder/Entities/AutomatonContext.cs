using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace AutomatonBuilder.Entities
{
    public class AutomatonContext
    {
        /// <summary>
        /// The number of nodes currently on the screen.
        /// </summary>
        public int NodeCount;

        /// <summary>
        /// A list containing all of the nodes currently on the screen
        /// </summary>
        public readonly List<ModelNode> NodesList;

        /// <summary>
        /// A stack storing the lines that can be undone
        /// </summary>
        public readonly Stack<List<Ellipse>> UndoStack;

        /// <summary>
        /// A stack storing the lines that can be redone
        /// </summary>
        public readonly Stack<List<Ellipse>> RedoStack;

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

        public AutomatonContext()
        {
            this.NodeCount = 0;
            this.StartingNode = null;
            this.NodesList = new List<ModelNode>();
            this.RedoStack = new Stack<List<Ellipse>>();
            this.UndoStack = new Stack<List<Ellipse>>();
            this.CurrentLine = new List<Ellipse>();
            this.LastRightClickPosition = new Point();
            this.IsLeftMouseKeyPressed = false;
            this.StartingNode = null;
        }
    }
}

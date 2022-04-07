using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatonBuilder.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Petzold.Media2D;
using System.Windows.Media;

namespace AutomatonBuilder.Entities
{
    public class AutomatonContext
    {
        public readonly List<ModelNode> NodesList;

        public readonly Stack<IAction> DoneActionsStack;

        public readonly Stack<IAction> UndoneActionsStack;

        public Polyline CurrentLine;

        public bool IsLeftMouseKeyPressed;

        public bool WasLeftMouseKeyPressedLastTick;

        public Point LeftClickHoldStartingPosition;

        public Point LeftClickHoldReleasePosition;

        public Point LastRightClickPosition;

        public ModelNode? StartingNode;

        public Canvas MainCanvas;

        public IMoveable? HoveredElement { get; set; }


        public AutomatonContext(Canvas windowCanvas)
        {
            this.StartingNode = null;
            this.NodesList = new List<ModelNode>();
            this.UndoneActionsStack = new Stack<IAction>();
            this.DoneActionsStack = new Stack<IAction>();
            this.CurrentLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2.5,
                Points = new PointCollection()
            };
            this.MainCanvas = windowCanvas;
            this.MainCanvas.Children.Add(this.CurrentLine);
            this.LastRightClickPosition = new Point();
            this.IsLeftMouseKeyPressed = false;
            this.StartingNode = null;
            this.HoveredElement = null;
        }

    }
}

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
using Newtonsoft.Json;

namespace AutomatonBuilder.Entities
{
    public class AutomatonContext
    {
        public class MouseProperties
        {
            public bool IsLeftMouseKeyPressed { get; set; }
            public bool WasLeftMouseKeyPressedLastTick { get; set; }
            public Point LeftClickHoldStartingPosition { get; set; }
            public Point LeftClickHoldReleasePosition { get; set; }
            public Point LastRightClickPosition { get; set; }
            public IMoveable? HoveredElement { get; set; }
        }

        public readonly List<ModelNode> NodesList;

        [JsonIgnore]
        public readonly Stack<IAction> DoneActionsStack;

        [JsonIgnore]
        public readonly Stack<IAction> UndoneActionsStack;

        [JsonIgnore]
        public Polyline CurrentLine { get; set; }

        [JsonIgnore]
        public MouseProperties MouseProperies { get; set; }

        public ModelNode? StartingNode;

        [JsonIgnore]
        public Canvas MainCanvas;

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
            this.MouseProperies = new MouseProperties
            {
                LastRightClickPosition = new Point(),
                IsLeftMouseKeyPressed = false,
                HoveredElement = null,
            };
            this.StartingNode = null;
            
        }

    }
}

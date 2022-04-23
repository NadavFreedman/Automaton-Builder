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
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Entities.Enums;

namespace AutomatonBuilder.Entities.Contexts
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

        public readonly List<BorderedText> BorderedTextsList;

        public readonly List<Polyline> DrawnLinesList;

        public readonly Stack<IAction> DoneActionsStack;

        public readonly Stack<IAction> UndoneActionsStack;

        public Polyline CurrentLine { get; set; }

        public MouseProperties MouseProperies { get; set; }

        public ModelNode? StartingNode;

        public Canvas MainCanvas;

        public AutomatonTypes type;

        public AutomatonContext(Canvas windowCanvas, MainEditingScreen? host, AutomatonTypes type)
        {
            this.StartingNode = null;
            this.NodesList = new List<ModelNode>();
            this.BorderedTextsList = new List<BorderedText>();
            this.DrawnLinesList = new List<Polyline>();
            this.UndoneActionsStack = new Stack<IAction>();
            this.DoneActionsStack = new Stack<IAction>();
            this.CurrentLine = new Polyline
            {
                Stroke = new SolidColorBrush(host.ColorPicker.SelectedColor!.GetValueOrDefault()),
                StrokeThickness = double.Parse(((ComboBoxItem)host.FontSizeComboBox.SelectedValue).Content.ToString()!),
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
            this.type = type;
        }

    }
}

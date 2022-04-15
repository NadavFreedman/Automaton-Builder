using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatonBuilder.Interfaces;
using System.Windows.Shapes;
using System.Windows.Media;
using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Contexts;
using System.Windows.Controls;

namespace AutomatonBuilder.Actions.DrawingActions
{
    public class DrawLineAction : IAction
    {
        private readonly Polyline drawnLine;
        private readonly MainEditingScreen host;
        private readonly AutomatonContext context;

        public DrawLineAction(AutomatonContext context, MainEditingScreen host)
        {
            this.context = context;
            this.drawnLine = context.CurrentLine!;
            this.host = host;
        }
        public void DoAction()
        {
            this.context.DrawnLinesList.Add(this.drawnLine);
            this.context.CurrentLine = new Polyline
            {
                Stroke = new SolidColorBrush(host.ColorPicker.SelectedColor!.GetValueOrDefault()),
                StrokeThickness = double.Parse(((ComboBoxItem)host.FontSizeComboBox.SelectedValue).Content.ToString()!),
                Points = new PointCollection()
            };
            this.context.MainCanvas.Children.Add(this.context.CurrentLine);
        }

        public void RedoAction()
        {
            this.context.MainCanvas.Children.Add(this.drawnLine);
            this.context.DrawnLinesList.Add(this.drawnLine);
        }

        public void UndoAction()
        {
            this.context.MainCanvas.Children.Remove(this.drawnLine);
            this.context.DrawnLinesList.Remove(this.drawnLine);
        }
    }
}

using AutomatonBuilder.Entities.Graphics;
using AutomatonBuilder.Entities.Nodes;
using AutomatonBuilder.Entities.TextElements;
using AutomatonBuilder.Interfaces;
using AutomatonBuilder.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AutomatonBuilder.Entities.Contexts
{
    public class JsonContext
    {
        [JsonProperty("nodes")]
        public List<NodeBrief> Nodes { get; set; }

        [JsonProperty("lines")]
        public List<PolylineBrief> Lines { get; set; }

        [JsonProperty("text_blocks")]
        public List<BorderedTextBrief> TextBlocks { get; set; }


        public JsonContext(AutomatonContext context)
        {
            this.Nodes = new List<NodeBrief>();
            this.TextBlocks = new List<BorderedTextBrief>();
            this.Lines = new List<PolylineBrief>();

            foreach (var node in context.NodesList)
            {
                this.Nodes.Add(new NodeBrief(node));
            }
            foreach (var text in context.BorderedTextsList)
            {
                this.TextBlocks.Add(new BorderedTextBrief(text));
            }
            foreach (var line in context.DrawnLinesList)
            {
                this.Lines.Add(new PolylineBrief(line));
            }
        }

        public JsonContext() { }

        public AutomatonContext ToContext(Canvas mainCanvas, MainEditingScreen host)
        {
            AutomatonContext context = new AutomatonContext(mainCanvas, host);

            LoadNodes(context, host);
            LoadConnections(context, host);
            LoadBorderedTexts(context, host);
            LoadLines(context);

            return context;

        }

        private void LoadNodes(AutomatonContext context, MainEditingScreen host)
        {
            foreach (var node in this.Nodes)
            {
                ModelNode newNode = new ModelNode(node.Index, host, node.Position);

                if (node.Accepting)
                    newNode.AcceptingClick(null, null);

                newNode.SetPosition(node.Position);

                newNode.MouseEnter += host.GeneralElement_MouseEnter;
                newNode.MouseLeave += host.Element_MouseLeave;

                NodeUtils.AddNodeToCanvas(context, newNode, host);
            }

            NodeUtils.SetNodesContextMenuOptions(context, host);
        }

        private void LoadConnections(AutomatonContext context, MainEditingScreen host)
        {
            foreach (var node in this.Nodes)
            {
                foreach (var connection in node.Connections)
                {
                    var source = ConnectorUtils.GetNodeByName(context, $"q{node.Index}");
                    var destination = ConnectorUtils.GetNodeByName(context, connection.ConnectedTo);
                    IConnector connector = ConnectorUtils.ConnectNodes(context, source, destination, connection.ConnectorData, host);

                    if (connector is IMoveable moveable)
                    {
                        moveable.SetPosition(connection.Position);
                    }
                }
            }
        }

        private void LoadBorderedTexts(AutomatonContext context, MainEditingScreen host)
        {
            foreach (var textBlock in this.TextBlocks)
            {
                BorderedText borderedText = new(textBlock.Properties);
                context.BorderedTextsList.Add(borderedText);

                TextUtils.AddContextMenuToText(borderedText, host);

                borderedText.AllowDragging(host);

                borderedText.SetPosition(textBlock.Position);

                borderedText.AddToCanvas(context.MainCanvas);
            }
        }

        private void LoadLines(AutomatonContext context)
        {
            foreach (var line in this.Lines)
            {
                context.CurrentLine = new Polyline
                {
                    Stroke = line.StrokeColor,
                    StrokeThickness = line.StorkeThickness,
                    Points = line.Points
                };
                context.MainCanvas.Children.Add(context.CurrentLine);
                context.DrawnLinesList.Add(context.CurrentLine);
            }
        }
    }
}

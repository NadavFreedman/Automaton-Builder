using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutomatonBuilder.Utils
{
    public static class PreviewScreenFactory
    {
        public static Canvas CreatePreviewCanvasByOption(SelectionPageOptions option)
        {
            switch (option)
            {
                case SelectionPageOptions.Open:
                    return CreateOpenProjectPreviewCanvas();
                case SelectionPageOptions.Basic:
                    return CreateBasicPreviewCanvas();
                case SelectionPageOptions.Pushdown:
                    return CreatePushdownPreviewCanvas();
                case SelectionPageOptions.Turing:
                    return CreateTuringPreviewCanvas();
                default:
                    break;
            }
            return new Canvas();
        }

        private static Canvas CreateBasicPreviewCanvas()
        {
            var basicPreviewCanvas = new Canvas();

            TextBlock previewText = new TextBlock();
            previewText.Text = "Create a new basic automaton.";
            previewText.FontSize = 18;
            basicPreviewCanvas.Children.Add(previewText);

            ModelNode previewStartingNode = new ModelNode(0, null, new Point(100, 150));
            basicPreviewCanvas.Children.Add(previewStartingNode.StartingArrow);
            basicPreviewCanvas.Children.Add(previewStartingNode);

            ModelNode previewAcceptingNode = new ModelNode(1, null, new Point(400, 150));
            previewAcceptingNode.AcceptingElipse.Visibility = Visibility.Visible;
            basicPreviewCanvas.Children.Add(previewAcceptingNode);

            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewAcceptingNode, "b");
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewStartingNode, "a,c");
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewAcceptingNode, "a,b,c");

            return basicPreviewCanvas;
        }

        private static Canvas CreatePushdownPreviewCanvas()
        {
            var basicPreviewCanvas = new Canvas();

            TextBlock previewText = new TextBlock();
            previewText.Text = "Create a new pushdown automaton.";
            previewText.FontSize = 18;
            basicPreviewCanvas.Children.Add(previewText);

            ModelNode previewStartingNode = new ModelNode(0, null, new Point(50, 150));
            basicPreviewCanvas.Children.Add(previewStartingNode.StartingArrow);
            basicPreviewCanvas.Children.Add(previewStartingNode);

            ModelNode previewAcumilatingANode = new ModelNode(1, null, new Point(200, 150));
            basicPreviewCanvas.Children.Add(previewAcumilatingANode);

            ModelNode previewAcumilatingBNode = new ModelNode(2, null, new Point(350, 150));
            basicPreviewCanvas.Children.Add(previewAcumilatingBNode);


            ModelNode previewAcceptingNode = new ModelNode(3, null, new Point(500, 150));
            previewAcceptingNode.AcceptingElipse.Visibility = Visibility.Visible;
            basicPreviewCanvas.Children.Add(previewAcceptingNode);

            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewAcumilatingANode, "a/S,▼A\na/A,▼A");
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewAcumilatingBNode, "b/A,▼B");


            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewAcumilatingANode, "a/⟂,▼A");
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewAcumilatingANode, previewAcumilatingBNode, "b/A,▲A");
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewAcumilatingBNode, previewAcceptingNode, "b/S,▲S");

            var longConnector = ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewAcumilatingANode, previewAcceptingNode, "b/S,▲S");
            longConnector.SetPosition(new Point(350, 250));


            return basicPreviewCanvas;
        }

        private static Canvas CreateTuringPreviewCanvas()
        {
            var basicPreviewCanvas = new Canvas();
            //△🡄 🡆
            TextBlock previewText = new TextBlock();
            previewText.Text = "Create a new turing machine.";
            previewText.FontSize = 18;
            basicPreviewCanvas.Children.Add(previewText);

            ModelNode previewStartingNode = new ModelNode(0, null, new Point(50, 180));
            basicPreviewCanvas.Children.Add(previewStartingNode.StartingArrow);
            basicPreviewCanvas.Children.Add(previewStartingNode);

            ModelNode previewStarted0Node = new ModelNode(1, null, new Point(180, 100));
            basicPreviewCanvas.Children.Add(previewStarted0Node);

            ModelNode previewStarted1Node = new ModelNode(2, null, new Point(180, 260));
            basicPreviewCanvas.Children.Add(previewStarted1Node);

            ModelNode last0Node = new ModelNode(3, null, new Point(370, 100));
            basicPreviewCanvas.Children.Add(last0Node);

            ModelNode last1Node = new ModelNode(4, null, new Point(370, 260));
            basicPreviewCanvas.Children.Add(last1Node);

            ModelNode previewAcceptingNode = new ModelNode(5, null, new Point(500, 180));
            previewAcceptingNode.AcceptingElipse.Visibility = Visibility.Visible;
            basicPreviewCanvas.Children.Add(previewAcceptingNode);

            var long0Connector = ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewStarted0Node, "0/0,🡆");
            long0Connector.SetPosition(new Point(50, 100));
            var long1Connector = ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewStarted1Node, "1/1,🡆");
            long1Connector.SetPosition(new Point(50, 260));

            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStarted0Node, last0Node, "△/△,🡄");
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStarted1Node, last1Node, "△/△,🡄");

            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewStarted0Node, "1/1,🡆\n0/0,🡆");
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewStarted1Node, "1/1,🡆\n0/0,🡆");

            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, last0Node, previewAcceptingNode, "0/0,🡆");
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, last1Node, previewAcceptingNode, "1/1,🡆");

            return basicPreviewCanvas;
        }

        private static Canvas CreateOpenProjectPreviewCanvas()
        {
            var openProjectPreviewCanvas = new Canvas();

            TextBlock previewText = new TextBlock();
            previewText.Text = "Load an EAF (Editable Automaton File) and continue working \non a project you've saved.";
            previewText.FontSize = 18;
            previewText.TextWrapping = TextWrapping.Wrap;
            openProjectPreviewCanvas.Children.Add(previewText);

            return openProjectPreviewCanvas;
        }
    }
}

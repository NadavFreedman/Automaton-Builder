using AutomatonBuilder.Entities;
using AutomatonBuilder.Entities.Connectors.ConnectorData;
using AutomatonBuilder.Entities.Connectors.ConnectorData.SingleData;
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

            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewAcceptingNode, new BasicAutomatonData("a"));
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewStartingNode, new BasicAutomatonData("a,c"));
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewAcceptingNode, new BasicAutomatonData("a,b,c"));

            return basicPreviewCanvas;
        }

        private static Canvas CreatePushdownPreviewCanvas()
        {
            var basicPreviewCanvas = new Canvas();

            TextBlock previewText = new TextBlock();
            previewText.Text = "Create a new pushdown automaton.";
            previewText.FontSize = 18;
            basicPreviewCanvas.Children.Add(previewText);

            ModelNode previewStartingNode = new ModelNode(0, null, new Point(100, 100));
            basicPreviewCanvas.Children.Add(previewStartingNode.StartingArrow);
            basicPreviewCanvas.Children.Add(previewStartingNode);

            ModelNode previewAcumilatingANode = new ModelNode(1, null, new Point(400, 100));
            basicPreviewCanvas.Children.Add(previewAcumilatingANode);

            ModelNode previewAcumilatingBNode = new ModelNode(2, null, new Point(400, 250));
            basicPreviewCanvas.Children.Add(previewAcumilatingBNode);


            ModelNode previewAcceptingNode = new ModelNode(3, null, new Point(100, 250));
            previewAcceptingNode.AcceptingElipse.Visibility = Visibility.Visible;
            basicPreviewCanvas.Children.Add(previewAcceptingNode);

            PushdownAutomatonData data = new PushdownAutomatonData(new List<SinglePushdownData>
            {
                new SinglePushdownData('a', 'S', PushdownActions.Push, 'A'),
                new SinglePushdownData('a', 'A', PushdownActions.Push, 'A'),
            });

            var selfConnector1 = ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewAcumilatingANode, data);
            selfConnector1.SetPosition(new Point(700, 100));

            data = new PushdownAutomatonData(new List<SinglePushdownData>
            {
                new SinglePushdownData('b', 'A', PushdownActions.Pop, 'A'),
            });

            var selfConnector2 = ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewAcumilatingBNode, data);
            selfConnector2.SetPosition(new Point(700, 250));


            data = new PushdownAutomatonData(new List<SinglePushdownData>
            {
                new SinglePushdownData('a', '⟂', PushdownActions.Push, 'S'),
            });
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewAcumilatingANode, data);

            data = new PushdownAutomatonData(new List<SinglePushdownData>
            {
                new SinglePushdownData('b', 'A', PushdownActions.Pop, 'A'),
            });
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewAcumilatingANode, previewAcumilatingBNode, data);

            data = new PushdownAutomatonData(new List<SinglePushdownData>
            {
                new SinglePushdownData('b', 'S', PushdownActions.Pop, 'S'),
            });
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewAcumilatingBNode, previewAcceptingNode, data);

            data = new PushdownAutomatonData(new List<SinglePushdownData>
            {
                new SinglePushdownData('b', 'S', PushdownActions.Pop, 'S'),
            });
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewAcumilatingANode, previewAcceptingNode, data);


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

            TuringAutomatonData data = new TuringAutomatonData(new List<SingleTuringData>
            {
                new SingleTuringData('0', '0', TuringActions.MoveRight),
            });
            var long0Connector = ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewStarted0Node, data);
            long0Connector.SetPosition(new Point(50, 100));
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, last0Node, previewAcceptingNode, data);

            data = new TuringAutomatonData(new List<SingleTuringData>
            {
                new SingleTuringData('1', '1', TuringActions.MoveRight),
            });
            var long1Connector = ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStartingNode, previewStarted1Node, data);
            long1Connector.SetPosition(new Point(50, 260));
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, last1Node, previewAcceptingNode, data);


            data = new TuringAutomatonData(new List<SingleTuringData>
            {
                new SingleTuringData('△', '△', TuringActions.MoveLeft),
            });
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStarted0Node, last0Node, data);
            ConnectorUtils.ConnectNodeToAnotherNode(basicPreviewCanvas, previewStarted1Node, last1Node, data);

            data = new TuringAutomatonData(new List<SingleTuringData>
            {
                new SingleTuringData('1', '1', TuringActions.MoveRight),
                new SingleTuringData('0', '0', TuringActions.MoveRight),
            });
            var connectToSelf = ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewStarted0Node, data);
            connectToSelf.SetPosition(new Point(380, 300));
            ConnectorUtils.ConnectNodeToSelf(basicPreviewCanvas, previewStarted1Node, data);

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

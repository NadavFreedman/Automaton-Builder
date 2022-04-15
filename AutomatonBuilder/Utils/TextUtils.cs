using AutomatonBuilder.Entities.Args;
using AutomatonBuilder.Entities.TextElements;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutomatonBuilder.Utils
{
    public class TextUtils
    {
        public static Border CreateBorderWithTextBlock(string text)
        {
            TextBlock lineTextBlock = new()
            {
                Background = Brushes.White,
                Text = text,
                Margin = new Thickness(3)
            };

            //Create a border
            Border border = new()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White,
                Child = lineTextBlock
            };

            border.Tag = lineTextBlock;

            return border;
        }

        public static Border CreateBorderWithTextBlock(AddTextArgs args)
        {
            TextBlock lineTextBlock = new()
            {
                Background = Brushes.White,
                Text = args.Text,
                Foreground = args.Color,
                FontSize = args.FontSize,
                TextAlignment = args.Alignment,
                FontWeight = args.Style,
                Margin = new Thickness(3)
            };

            //Create a border
            Border border = new()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White,
                Child = lineTextBlock
            };

            border.Tag = lineTextBlock;

            return border;
        }
        public static FormattedText CreateFormattedText(TextBlock block)
        {
            return new FormattedText(
                                     block.Text,
                                     CultureInfo.CurrentCulture,
                                     FlowDirection.LeftToRight,
                                     new Typeface(block.FontFamily, block.FontStyle, block.FontWeight, block.FontStretch),
                                     block.FontSize,
                                     Brushes.Black,
                                     new NumberSubstitution(),
                                     1);
        }

        public static void AddContextMenuToText(BorderedText borderedText, MainEditingScreen host)
        {
            MenuItem removeConnector = new MenuItem
            {
                Header = "Remove",
                Tag = borderedText
            };
            removeConnector.Click += host.RemoveText_Click;
            var menu = new ContextMenu();
            menu.Items.Add(removeConnector);
            borderedText.AttachContextMenu(menu);
        }
    }
}

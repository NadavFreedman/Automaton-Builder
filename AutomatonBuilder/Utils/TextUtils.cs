using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            lineTextBlock.Tag = border;

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

        public static void SetPositionForText(Border borderedText, Point textCoords, FormattedText? formattedText = null)
        {
            if (formattedText == null)
                formattedText = TextUtils.CreateFormattedText((TextBlock)borderedText.Child);
            Canvas.SetLeft(borderedText, textCoords.X - 2 - formattedText.Width / 2);
            Canvas.SetTop(borderedText, textCoords.Y - 2 - formattedText.Height / 2);
        }

        public static void RemoveBorderedElementFromCanvas(Border box, Canvas mainCanvas)
        {
            mainCanvas.Children.Remove(box);
        }
        public static void AddBorderedElementToCanvas(Border box, Canvas mainCanvas)
        {
            mainCanvas.Children.Add(box);
        }
    }
}

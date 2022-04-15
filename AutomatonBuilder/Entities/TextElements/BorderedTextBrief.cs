using AutomatonBuilder.Entities.Args;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomatonBuilder.Entities.TextElements
{
    public class BorderedTextBrief
    {
        [JsonProperty("properties")]
        public AddTextArgs Properties { get; set; }

        [JsonProperty("position")]
        public Point Position { get; set; }

        public BorderedTextBrief(BorderedText borderedText)
        {
            this.Properties = new AddTextArgs
            {
                Text = borderedText.Block.Text,
                FontSize = borderedText.Block.FontSize,
                Alignment = borderedText.Block.TextAlignment,
                Color = borderedText.Block.Foreground,
                Style = borderedText.Block.FontWeight
            };
            this.Position = borderedText.GetPosition();
        }
        public BorderedTextBrief() {}
    }
}

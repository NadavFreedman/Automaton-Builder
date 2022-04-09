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
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("position")]
        public Point Position { get; set; }

        [JsonProperty("font_size")]
        public double FontSize { get; set; }

        public BorderedTextBrief(BorderedText borderedText)
        {
            this.Text = borderedText.GetText();
            this.Position = borderedText.GetPosition();
            this.FontSize = 12;
        }
        public BorderedTextBrief() {}
    }
}

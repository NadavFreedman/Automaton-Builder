using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutomatonBuilder.Entities.Graphics
{
    public class PolylineBrief
    {
        [JsonProperty("points")]
        public PointCollection Points { get; set; }

        [JsonProperty("stroke_thickness")]
        public double StorkeThickness { get; set; }

        [JsonProperty("stroke_color")]
        public Brush StrokeColor { get; set; }

        public PolylineBrief() { }

        public PolylineBrief(Polyline drawnLine)
        {
            this.Points = drawnLine.Points;
            this.StorkeThickness = drawnLine.StrokeThickness;
            this.StrokeColor = drawnLine.Stroke;
        }
    }
}

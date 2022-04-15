using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AutomatonBuilder.Entities.Args
{
    public class AddTextArgs
    {
        public string Text { get; set; }
        public Brush Color { get; set; }
        public double FontSize { get; set; }
        public TextAlignment Alignment { get; set; }
        public FontWeight Style { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutomatonBuilder.Interfaces
{
    public interface IConnector
    {
        public string ConnectorData { get; }

        public void ChangeConnectorData(string text);

        public void SetConnectorStart(Point startingPoint);
        public void SetConnectorEnd(Point endPoint);

        public void AddToCanvasButtom(Canvas canvas);

        public void RemoveFromCanvas(Canvas canvas);

        public void BindConnectorToMainWindow(MainWindow mainWindow);
    }
}

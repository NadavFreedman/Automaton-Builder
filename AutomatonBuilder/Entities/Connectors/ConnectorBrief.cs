using AutomatonBuilder.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomatonBuilder.Entities.Connectors
{
    public class ConnectorBrief
    {
        [JsonProperty("connector_data")]
        public string ConnectorData { get; set; }

        [JsonProperty("position")]
        public Point Position { get; set; }

        [JsonProperty("connected_to")]
        public string ConnectedTo { get; set; }

        public ConnectorBrief(KeyValuePair<IConnector, ModelNode> connector)
        {
            this.ConnectorData = connector.Key.ConnectorData;
            this.ConnectedTo = connector.Value.ToString();
            if (connector.Key is IMoveable moveableConnector)
                this.Position = moveableConnector.GetPosition();
        }

        public ConnectorBrief() { }
    }
}

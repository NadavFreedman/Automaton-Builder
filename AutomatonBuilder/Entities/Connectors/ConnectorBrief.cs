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
        public IConnectorData ConnectorData { get; set; }

        [JsonProperty("position")]
        public Point Position { get; set; }

        [JsonProperty("connected_to")]
        public string ConnectedTo { get; set; }

        public ConnectorBrief(KeyValuePair<ModelNode, IConnector> connector)
        {
            this.ConnectorData = connector.Value.ConnectorData;
            this.ConnectedTo = connector.Key.ToString();
            if (connector.Value is IMoveable moveableConnector)
                this.Position = moveableConnector.GetPosition();
        }

        public ConnectorBrief() { }
    }
}

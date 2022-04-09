using AutomatonBuilder.Entities.Connectors;
using AutomatonBuilder.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomatonBuilder.Entities.Nodes
{
    public class NodeBrief
    {
        [JsonProperty("position")]
        public Point Position;
        [JsonProperty("index")]
        public int Index;
        [JsonProperty("connections")]
        public List<ConnectorBrief> Connections;
        [JsonProperty("starting")]
        public bool Starting { get; set; }
        [JsonProperty("accepting")]
        public bool Accepting { get; set; }


        public NodeBrief(ModelNode node)
        {
            this.Position = node.GetPosition();
            this.Index = node.Index;
            this.Connections = new List<ConnectorBrief>();
            foreach (var connector in node.ConnectedLinesFromThisNode)
            {
                this.Connections.Add(new ConnectorBrief(connector));
            }
            this.Starting = node.Starting;
            this.Accepting = node.Accepting;
        }

        public NodeBrief() { }
    }
}

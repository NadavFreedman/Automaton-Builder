using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Interfaces
{
    public interface IConnectionModal
    {
        public IConnectorData? ConnectorData { get; set; }

        public bool? ShowDialog();

        public bool? DialogResult { get; set; }
    }
}

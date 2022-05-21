using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonBuilder.Entities.Exceptions
{
    public class BuilderUnsupportedTypeException : Exception
    {
        public BuilderUnsupportedTypeException(string message) : base(message) { }
    }
}

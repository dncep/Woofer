using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem
{
    public class BadMethodReturnException : Exception
    {
        public BadMethodReturnException() : base() { }
        public BadMethodReturnException(string message) : base(message) { }
        public BadMethodReturnException(string message, Exception innerException) : base(message, innerException) { }
    }
}

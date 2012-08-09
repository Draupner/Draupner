using System;
using System.Runtime.Serialization;

namespace Scaffold.Exceptions
{
    public class GeneratorNotFoundException : Exception
    {
        public GeneratorNotFoundException()
        {
        }

        public GeneratorNotFoundException(string message) : base(message)
        {
        }

        public GeneratorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GeneratorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

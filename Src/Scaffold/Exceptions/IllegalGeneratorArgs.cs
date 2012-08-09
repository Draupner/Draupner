using System;
using System.Runtime.Serialization;

namespace Scaffold.Exceptions
{
    public class IllegalGeneratorArgs : Exception
    {
        public IllegalGeneratorArgs()
        {
        }

        public IllegalGeneratorArgs(string message) : base(message)
        {
        }

        public IllegalGeneratorArgs(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalGeneratorArgs(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
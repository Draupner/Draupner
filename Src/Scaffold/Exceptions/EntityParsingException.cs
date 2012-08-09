using System;
using System.Runtime.Serialization;

namespace Scaffold.Exceptions
{
    public class EntityParsingException : Exception
    {
        public EntityParsingException()
        {
        }

        public EntityParsingException(string message) : base(message)
        {
        }

        public EntityParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
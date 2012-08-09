using System;
using System.Runtime.Serialization;

namespace Scaffold.Exceptions
{
    public class ConfigurationFileNotFoundException : Exception
    {
        public ConfigurationFileNotFoundException()
        {
        }

        public ConfigurationFileNotFoundException(string message) : base(message)
        {
        }

        public ConfigurationFileNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigurationFileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

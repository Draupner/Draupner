using System;
using System.Runtime.Serialization;

namespace Scaffold.Exceptions
{
    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException()
        {
        }

        public TemplateNotFoundException(string message) : base(message)
        {
        }

        public TemplateNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TemplateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace Support.Domain.Exceptions
{
    [Serializable]
    public class EmailNotSendException : Exception
    {
        public EmailNotSendException() : base("emailNotSendException") { }

        public EmailNotSendException(string message)
          : base(message)
        {
        }

        public EmailNotSendException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected EmailNotSendException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

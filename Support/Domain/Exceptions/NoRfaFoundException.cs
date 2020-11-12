using System;
using System.Runtime.Serialization;

namespace Support.Domain.Exceptions
{
    [Serializable]
    public class NoRfaFoundException : Exception
    {
        public NoRfaFoundException() : base("nothingRfaException") { }

        public NoRfaFoundException(string message)
           : base(message)
        {
        }

        public NoRfaFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected NoRfaFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

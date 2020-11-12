using System;
using System.Runtime.Serialization;

namespace Common.Domain.Emails
{
    [Serializable]
    public class InvalidMailFormatException : Exception
    {
        public InvalidMailFormatException() : base("Invalid mail format") { }

        public InvalidMailFormatException(string message)
           : base(message)
        {
        }

        public InvalidMailFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidMailFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

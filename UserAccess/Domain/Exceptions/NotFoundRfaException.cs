using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
    public class NotFoundRfaException : Exception
    {
        public NotFoundRfaException()
            :base("invalidPhoneNumberException")
        {
        }

        public NotFoundRfaException(string message)
            : base(message)
        {
        }

        public NotFoundRfaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected NotFoundRfaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
    public class UserNotFoundForUpdateException : Exception
    {
        public UserNotFoundForUpdateException() : base("userNotFoundForUpdateException") { }

        public UserNotFoundForUpdateException(string message)
           : base(message)
        {
        }

        public UserNotFoundForUpdateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected UserNotFoundForUpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

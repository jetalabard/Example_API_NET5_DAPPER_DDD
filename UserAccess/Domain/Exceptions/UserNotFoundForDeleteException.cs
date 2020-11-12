using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
    public class UserNotFoundForDeleteException : Exception
    {
        public UserNotFoundForDeleteException() : base("userNotFoundForDeleteException") { }

        public UserNotFoundForDeleteException(string message)
   : base(message)
        {
        }

        public UserNotFoundForDeleteException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected UserNotFoundForDeleteException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

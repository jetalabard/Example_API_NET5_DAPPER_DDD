using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
    public class RoleNotFoundForAddUserException : Exception
    {
        public RoleNotFoundForAddUserException() : base("roleNotFoundForAddUserException") { }

        public RoleNotFoundForAddUserException(string message)
: base(message)
        {
        }

        public RoleNotFoundForAddUserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected RoleNotFoundForAddUserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

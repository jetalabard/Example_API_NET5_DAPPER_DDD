using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
    public class RoleNotFoundForUpdateUserException : Exception
    {
        public RoleNotFoundForUpdateUserException() : base("roleNotFoundForUpdateUserException") { }

        public RoleNotFoundForUpdateUserException(string message)
   : base(message)
        {
        }

        public RoleNotFoundForUpdateUserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected RoleNotFoundForUpdateUserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

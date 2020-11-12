using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
    public class EmailAlreadyAffectedException : Exception
    {
        public EmailAlreadyAffectedException() : base("emailAlreadyAffectedException") { }


        public EmailAlreadyAffectedException(string message)
: base(message)
        {
        }

        public EmailAlreadyAffectedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected EmailAlreadyAffectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace UserAccess.Domain.Exceptions
{
    [Serializable]
   public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException() : base("Invalid phone number") { }

        public InvalidPhoneNumberException(string message)
         : base(message)
        {
        }

        public InvalidPhoneNumberException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected InvalidPhoneNumberException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

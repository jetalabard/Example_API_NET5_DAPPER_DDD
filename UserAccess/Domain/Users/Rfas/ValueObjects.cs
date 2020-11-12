using System;
using System.Text.RegularExpressions;
using Common.Domain.BuildingBlocks;
using UserAccess.Domain.Exceptions;

namespace UserAccess.Domain.Users.Rfas
{
    public record RfaId : SingleValueObject<Guid>
    {
        public RfaId(Guid value) : base(value)
        {
        }
    }

    public record PhoneNumber : SingleValueObject<string>
    {
        private const string PhonePattern = @"^(\+)?([ 0-9]){10,16}$";

        public PhoneNumber(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                !Regex.IsMatch(value, PhonePattern))
            {
                throw new InvalidPhoneNumberException();
            }
        }

    }

    public record Profession : SingleValueObject<string>
    {
        public Profession(string value) : base(value)
        {
        }
    }
}

using Common.Domain.BuildingBlocks;
using System;

namespace UserAccess.Domain.Users
{
    public record UserId : SingleValueObject<Guid>
    {
        public UserId(Guid value) : base(value)
        {
        }
    }

    public record FirstName : SingleValueObject<string>
    {
        public FirstName(string value) : base(value)
        {
        }
    }

    public record LastName : SingleValueObject<string>
    {
        public LastName(string value) : base(value)
        {
        }
    }

    public record IsActive : SingleValueObject<bool>
    {
        public IsActive(bool value) : base(value)
        {
        }
    }
}

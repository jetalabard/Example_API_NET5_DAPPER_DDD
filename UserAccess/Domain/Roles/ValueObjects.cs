using Common.Domain.BuildingBlocks;
using System;

namespace UserAccess.Domain.Roles
{
    public record Name : SingleValueObject<string>
    {
        public Name(string value) : base(value)
        {
        }
    }

    public record Permission : SingleValueObject<Int16>
    {
        public Permission(Int16 value) : base(value)
        {
        }
    }
}

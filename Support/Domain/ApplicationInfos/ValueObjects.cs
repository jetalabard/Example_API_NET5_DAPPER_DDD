using System;
using Common.Domain.BuildingBlocks;

namespace Support.Domain.ApplicationInfos
{
    public record ApplicationInfoId : SingleValueObject<Guid>
    {
        public ApplicationInfoId(Guid value) : base(value)
        {
        }
    }

    public record NameApp : SingleValueObject<string>
    {
        public NameApp(string value) : base(value)
        {
        }
        public override string ToString() => Value;
    }

    public record VersionApp : SingleValueObject<string>
    {
        public VersionApp(string value) : base(value)
        {
        }
        public override string ToString() => Value;
    }

    public record CodeApp : SingleValueObject<string>
    {
        public CodeApp(string value) : base(value)
        {
        }
        public override string ToString() => Value;
    }
}

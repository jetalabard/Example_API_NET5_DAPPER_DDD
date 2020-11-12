using Common.Domain.BuildingBlocks;

namespace UserAccess.Domain.Roles
{
    public record Role(Name Name, Permission Permissions) : ValueObject;
}

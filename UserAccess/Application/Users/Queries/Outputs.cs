using System;

namespace UserAccess.Application.Users.Outputs
{
    public record UserRequest(Guid Id, string FirstName, string LastName, string Email, string Name, Guid Code, Int16 Permission, bool IsActive);

    public record UserOutput(Guid Id, string LastName, string FirstName, string Email, bool IsActive, RoleOutput Role);

    public record RoleOutput(string Name, Int16 Permission);
}

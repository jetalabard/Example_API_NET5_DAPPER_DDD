using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserAccess.Application.Users;
using UserAccess.Domain.Roles;

namespace UserAccess.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserAccessContext _userAccessContext;

        public RoleRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }

        public async Task<Role> GetRole(string name)
        {
            return await _userAccessContext.Roles.FirstOrDefaultAsync(x => x.Name == new Name(name));
        }
    }
}

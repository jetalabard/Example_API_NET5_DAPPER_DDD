using System.Threading.Tasks;
using UserAccess.Domain.Roles;

namespace UserAccess.Application.Users
{
    public interface IRoleRepository
    {
        Task<Role> GetRole(string name);
    }
}

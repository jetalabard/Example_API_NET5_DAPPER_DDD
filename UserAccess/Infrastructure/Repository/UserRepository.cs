using System.Linq;
using System.Threading.Tasks;
using Common.Domain.Emails;
using Microsoft.EntityFrameworkCore;
using UserAccess.Application.Users;
using UserAccess.Domain.Users;

namespace UserAccess.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAccessContext _userAccessContext;

        public UserRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }

        public async Task AddAsync(User user)
        {
            _userAccessContext.Add(user);
            await _userAccessContext.SaveChangesAsync();
        }

        public async Task<bool> CheckMailExist(UserId id, Email email)
        {
            return await _userAccessContext.Users.AnyAsync(x => x.Email == email && x.Id != id);
        }

        public async Task<User> GetById(UserId id)
        {
            return (await _userAccessContext.Users
               .Include(x => x.Role)
               .ToListAsync())
               .FirstOrDefault(x => x.Id == id);
        }

        public async Task Update(User user)
        {
            _userAccessContext.Update(user);
            await _userAccessContext.SaveChangesAsync();
        }

        public async Task Delete(User userToDelete)
        {
            _userAccessContext.Remove(userToDelete);
            await _userAccessContext.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(Email id)
        {
            return (await _userAccessContext.Users
               .ToListAsync())
               .FirstOrDefault(x => x.Email == id);
        }
    }
}

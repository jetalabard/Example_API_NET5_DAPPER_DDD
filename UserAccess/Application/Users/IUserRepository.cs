using Common.Domain.Emails;
using System.Threading.Tasks;
using UserAccess.Domain.Users;

namespace UserAccess.Application.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task Update(User user);

        Task<User> GetById(UserId id);

        Task<User> GetByEmail(Email id);

        Task<bool> CheckMailExist(UserId id, Email email);

        Task Delete(User userToDelete);
    }
}

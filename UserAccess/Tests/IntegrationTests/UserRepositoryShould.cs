using Xunit;
using UserAccess.Application.Users;
using UserAccess.Infrastructure.Repository;
using UserAccess.IntegrationTests.Helpers;
using FluentAssertions;
using UserAccess.Domain.Users;
using System.Linq;
using System;
using Common.Domain.Emails;
using System.Threading.Tasks;

namespace UserAccess.Tests.IntegrationTests
{
    public class UserRepositoryShould : IntegrationTestUserAccess
    {
        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        public UserRepositoryShould() : base()
        {
            _userRepository = new UserRepository(ExampleContext);
            _roleRepository = new RoleRepository(ExampleContext);
        }

        [Fact]
        public async Task ShouldGetUserById()
        {

            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));
            var roleGet = await _roleRepository.GetRole(role.Name.Value);
            var id = Guid.NewGuid();
            await _userRepository.AddAsync(User.Create(new UserId(id), new FirstName("test"), new LastName("test"), new Email("test@test.fr"), roleGet));
            var userTest = await _userRepository.GetById(new UserId(id));

            userTest.Should().NotBeNull();
            userTest.Email.Value.Should().Be("test@test.fr");
        }

        [Fact]
        public async Task ShouldGetRole()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            var roleGet = await _roleRepository.GetRole(role.Name.Value);

            roleGet.Should().NotBeNull();
            roleGet.Name.Value.Should().Be("rfa");
        }

        [Fact]
        public async Task ShouldAddUser()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));
            var roleGet = await _roleRepository.GetRole(role.Name.Value);
            var id = Guid.NewGuid();
            await _userRepository.AddAsync(User.Create(new UserId(id), new FirstName("test"), new LastName("test"), new Email("test@test.fr"), roleGet));

            var userGet = await _userRepository.GetById(new UserId(id));
            userGet.Should().NotBeNull();
            userGet.FirstName.Value.Should().Be("test");

            ExampleContext.Remove(userGet);
            await ExampleContext.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldDeleteUser()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));
            var roleGet = await _roleRepository.GetRole(role.Name.Value);
            var id = Guid.NewGuid();
            await _userRepository.AddAsync(User.Create(new UserId(id), new FirstName("test"), new LastName("test"), new Email("test@test.fr"), roleGet));

            var userGet = await _userRepository.GetById(new UserId(id));
            await _userRepository.Delete(userGet);

            userGet = await _userRepository.GetById(new UserId(id));
            userGet.Should().BeNull();
        }

    }
}

using Xunit;
using System;
using UserAccess.Application.Users;
using UserAccess.Infrastructure.Repository;
using UserAccess.IntegrationTests.Helpers;
using FluentAssertions;
using UserAccess.Domain.Users;
using System.Linq;
using UserAccess.Application.Users.Commands;
using UserAccess.Domain.Exceptions;
using System.Threading.Tasks;
using Common.Domain.Emails;

namespace UserAccess.IntegrationTests
{
    public class UserCommandHandlerShould : IntegrationTestUserAccess
    {
        private readonly UserCommandHandler _userCommandHandler;

        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        public UserCommandHandlerShould() : base()
        {            
            _userRepository = new UserRepository(ExampleContext);
            _roleRepository = new RoleRepository(ExampleContext);

            _userCommandHandler = new UserCommandHandler(_userRepository, _roleRepository, new RfaRepository(ExampleContext));
        }


        [Fact]
        public async Task ShouldCreateDefaultUser()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));
            var addUserCommand = new AddUserCommand( "user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));

            var userAdded = await _userCommandHandler.Handle(addUserCommand, default);

            var userToTest = await _userRepository.GetById(new UserId(userAdded.Id));
            userToTest.Should().NotBeNull();
            userToTest.FirstName.Value.Should().Be("user");
            userToTest.Role.Name.Value.Should().Be("rfa");


            ExampleContext.Users.Remove(userToTest);
            await ExampleContext.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldUpdateDefaultUser()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            // add user
            var addUserCommand = new AddUserCommand("user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));
            var userAdded = await _userCommandHandler.Handle(addUserCommand, default);

            var projectManagerRole = ExampleContext.Roles
                .FirstOrDefault(x => x.Name == new Domain.Roles.Name("admin"));
            var UpdatedUserCommand = new UpdateUserCommand(userAdded.Id, "updated", "updated", "user@user.fr", 
                new RoleUser(projectManagerRole.Name.Value, projectManagerRole.Permissions.Value));

            await _userCommandHandler.Handle(UpdatedUserCommand, default);

            var userToTest = await _userRepository.GetById(new UserId(UpdatedUserCommand.Id));
            userToTest.Should().NotBeNull();
            userToTest.FirstName.Value.Should().Be("updated");
            userToTest.Role.Name.Value.Should().Be("admin");

            ExampleContext.Users.Remove(userToTest);
            await ExampleContext.SaveChangesAsync();
        }

        [Fact]
        public void ShouldCreateUserWithInvalidMail()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            // add user
            var addUserCommand = new AddUserCommand("user", "user", "invalidEmail", new RoleUser(role.Name.Value, role.Permissions.Value));

            Func<Task> comparison = async () =>
            {
               await _userCommandHandler.Handle(addUserCommand, default);
            };


            comparison.Should().Throw<InvalidMailFormatException>();
        }


        [Fact]
        public async Task ShouldUpdateUserWithInvalidMail()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            // add user
            var addUserCommand = new AddUserCommand("user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));
            var userAdded = await _userCommandHandler.Handle(addUserCommand, default);

            var projectManagerRole = ExampleContext.Roles
                .FirstOrDefault(x => x.Name == new Domain.Roles.Name("admin"));
            var UpdatedUserCommand = new UpdateUserCommand(userAdded.Id, "updated", "updated", "invalidEmail",
                new RoleUser(projectManagerRole.Name.Value, projectManagerRole.Permissions.Value));

            Func<Task> comparison = async () =>
            {
                await _userCommandHandler.Handle(UpdatedUserCommand, default);
            };

            comparison.Should().Throw<InvalidMailFormatException>();

            ExampleContext.Users.Remove(ExampleContext.Users.Find(new UserId(userAdded.Id)));
            await ExampleContext.SaveChangesAsync();
        }


        [Fact]
        public async Task ShouldCreateUserWithEmailAlreadyExist()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            // add user
            var addUserCommand = new AddUserCommand("user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));
            var userAdded = await _userCommandHandler.Handle(addUserCommand, default);


            // add user
            addUserCommand = new AddUserCommand("user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));

            Func<Task> comparison = async () =>
            {
                await _userCommandHandler.Handle(addUserCommand, default);
            };

            comparison.Should().Throw<EmailAlreadyAffectedException>();

            ExampleContext.Users.Remove(ExampleContext.Users.Find(new UserId(userAdded.Id)));
            await ExampleContext.SaveChangesAsync();
        }



        [Fact]
        public async Task ShouldUpdateUserWithEmailAlreadyExist()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            // add user
            var addUserCommand = new AddUserCommand("user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));
            var userAdded1 = await _userCommandHandler.Handle(addUserCommand, default);

            // add user
            addUserCommand = new AddUserCommand("user", "user", "test@test.fr", new RoleUser(role.Name.Value, role.Permissions.Value));
            var userAdded2 = await _userCommandHandler.Handle(addUserCommand, default);


            // add user
            var updateUserCommand = new UpdateUserCommand(userAdded2.Id,"user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));

            Func<Task> comparison = async () =>
            {
                await _userCommandHandler.Handle(updateUserCommand, default);
            };

            comparison.Should().Throw<EmailAlreadyAffectedException>();

            ExampleContext.Users.Remove(ExampleContext.Users.Find(new UserId(userAdded2.Id)));
            ExampleContext.Users.Remove(ExampleContext.Users.Find(new UserId(userAdded1.Id)));
            await ExampleContext.SaveChangesAsync();
        }

        [Fact]
        public async Task ShoulDeleteUser()
        {
            var role = ExampleContext.Roles.FirstOrDefault(x => x.Name == new Domain.Roles.Name("rfa"));

            // add user
            var addUserCommand = new AddUserCommand("user", "user", "user@user.fr", new RoleUser(role.Name.Value, role.Permissions.Value));
            var userAdded = await _userCommandHandler.Handle(addUserCommand, default);

            var deleteCommand = new DeleteUserCommand(userAdded.Id);

            var result = await _userCommandHandler.Handle(deleteCommand, default);
            result.Should().BeTrue();

        }


        [Fact]
        public void ShoulDeleteNotExistUser()
        {
            var deleteCommand = new DeleteUserCommand(Guid.NewGuid());
            Func<Task> comparison = async () => await _userCommandHandler.Handle(deleteCommand, default);
            comparison.Should().Throw<UserNotFoundForDeleteException>();
        }
    }
}

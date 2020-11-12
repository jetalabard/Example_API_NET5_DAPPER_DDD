using Xunit;
using System.Threading.Tasks;
using System;
using Dapper;
using Common.Domain.Emails;
using FluentAssertions;
using UserAccess.IntegrationTests.Helpers;
using UserAccess.Application.Rfas;
using UserAccess.Infrastructure.Repository;
using UserAccess.Application.Rfas.Commands;
using System.Linq;

namespace UserAccess.IntegrationTests
{
    public class RfaCommandHandlerShould : IntegrationTestUserAccess
    {
        private readonly RfaCommandHandler _rfaCommandHandler;

        private readonly IRfaRepository _rfaRepository;

        public RfaCommandHandlerShould() : base()
        {
            _rfaRepository = new RfaRepository(ExampleContext);
            _rfaCommandHandler = new RfaCommandHandler(_rfaRepository, new UserRepository(ExampleContext), new RoleRepository(ExampleContext));
        }

        [Fact]
        public async Task AddRfa()
        {
            var email = "testAddRfa@test.fr";
            var role = ExampleContext.Roles.AsEnumerable().FirstOrDefault(x => x.Name.Value == "rfa");
            
            var addrfaCommand = new AddRfaCommand(new AddUserRfaCommand( "test", "test", email, new RoleUserRfa(role.Name.Value,  role.Permissions.Value)), "0678925355", "profession");
            await _rfaCommandHandler.Handle(addrfaCommand, default);

            var rfaAdded = await _rfaRepository.GetRfaByEmail(new Email(email));
            rfaAdded.Should().NotBeNull();
            rfaAdded.Profession.Value.Should().Be("profession");

            Dbconnection.Execute("DELETE FROM RFAINFO where email = @Email", new { Email = email });
            Dbconnection.Execute("DELETE FROM [User] where email = @Email", new { Email = email });
        }

        [Fact]
        public async Task UpdateRfa()
        {
            var email = "testUpdate@test.fr";

            var role = ExampleContext.Roles.AsEnumerable().FirstOrDefault(x => x.Name.Value == "rfa");

            var addrfaCommand = new AddRfaCommand(new AddUserRfaCommand("test", "test", email, new RoleUserRfa(role.Name.Value, role.Permissions.Value)), "0678925355", "profession");
            await _rfaCommandHandler.Handle(addrfaCommand, default);

            var id = (await _rfaRepository.GetRfaByEmail(new Email(email))).UserId.Value;

            var updateCommand = new UpdateRfaCommand(new UpdateUserRfaCommand(id, "test", "test", email, 
                new RoleUserRfa("rfa", 33)), "0678925355", "professionUpdated", email);
            await _rfaCommandHandler.Handle(updateCommand, default);

            var rfaAdded = await _rfaRepository.GetRfaByEmail(new Email(email));
            rfaAdded.Should().NotBeNull();
            rfaAdded.Profession.Value.Should().Be("professionUpdated");

            Dbconnection.Execute("DELETE FROM RFAINFO where email = @Email", new { Email = email });
            Dbconnection.Execute("DELETE FROM [User] where email = @Email", new { Email = email });
        }


        [Fact]
        public async Task DeleteRfa()
        {
            var email = "testDelete@test.fr";
            var role = ExampleContext.Roles.AsEnumerable().FirstOrDefault(x => x.Name.Value == "rfa");

            var addrfaCommand = new AddRfaCommand(new AddUserRfaCommand("test", "test", email, new RoleUserRfa(role.Name.Value, role.Permissions.Value)), "0678925355", "profession");
             await _rfaCommandHandler.Handle(addrfaCommand, default);
                       
            await _rfaCommandHandler.Handle(new DeleteRfaCommand(email), default);

            var rfaAdded = await _rfaRepository.GetRfaByEmail(new Email(email));
            rfaAdded.Should().BeNull();

            Dbconnection.Execute("DELETE FROM [User] where email = @Email", new { Email = email });
        }
    }
}

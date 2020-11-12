using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain.Emails;
using MediatR;
using UserAccess.Application.Users;
using UserAccess.Domain.Exceptions;
using UserAccess.Domain.Users;
using UserAccess.Domain.Users.Rfas;

namespace UserAccess.Application.Rfas.Commands
{
    public record AddRfaCommand(AddUserRfaCommand User, string PhoneNumber, string Profession) : IRequest;

    public record UpdateRfaCommand(UpdateUserRfaCommand User, string PhoneNumber, string Profession, string Email) : IRequest;

    public record UpdateUserRfaCommand(Guid Id, string FirstName, string LastName, string Email, RoleUserRfa Role);

    public record AddUserRfaCommand(string FirstName, string LastName, string Email, RoleUserRfa Role);

    public record RoleUserRfa(string Name, int Permission);

    public record DeleteRfaCommand(string Email) : IRequest;

    public class RfaCommandHandler : IRequestHandler<AddRfaCommand>, IRequestHandler<UpdateRfaCommand>, IRequestHandler<DeleteRfaCommand>
    {
        private readonly IRfaRepository _repository;

        private readonly IUserRepository _repositoryUser;

        private readonly IRoleRepository _roleRepository;

        public RfaCommandHandler(IRfaRepository repository, IUserRepository repositoryUser, IRoleRepository roleRepository)
        {
            _repository = repository;
            _repositoryUser = repositoryUser;
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(AddRfaCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRole(request.User.Role.Name);

            if (role == null)
            {
                throw new RoleNotFoundForAddUserException();
            }


            var email = new Email(request.User.Email);
            var id = new UserId(Guid.NewGuid());
            var existEmail = await _repositoryUser.CheckMailExist(id, email);

            if (existEmail)
            {
                throw new EmailAlreadyAffectedException();
            }

            var user = User.Create(
                id,
                new FirstName(request.User.FirstName),
                new LastName(request.User.LastName),
                email,
                role
                );

            await _repositoryUser.AddAsync(user);

            var rfa = Rfa.Create(
                user.Id,
                new Email(request.User.Email),
                new PhoneNumber(request.PhoneNumber),
                new Profession(request.Profession));

            await _repository.Add(rfa);

            return new Unit();
        }

        public async Task<Unit> Handle(UpdateRfaCommand request, CancellationToken cancellationToken)
        {
            var id = new UserId(request.User.Id);
            var userToUpdated = await _repositoryUser.GetById(id);
            if (userToUpdated == null)
            {
                throw new UserNotFoundForUpdateException();
            }

            var role = await _roleRepository.GetRole(request.User.Role.Name);
            if (role == null)
            {
                throw new RoleNotFoundForUpdateUserException();
            }

            var email = new Email(request.Email);
            var existEmail = await _repositoryUser.CheckMailExist(id, email);

            if (existEmail)
            {
                throw new EmailAlreadyAffectedException();
            }

            userToUpdated.UpdateFirstName(request.User.FirstName);
            userToUpdated.UpdateLastName(request.User.LastName);
            userToUpdated.UpdateEmail(request.Email);
            userToUpdated.UpdateRole(role);

            await _repositoryUser.Update(userToUpdated);

            var rfa = await _repository.GetRfaByEmail(userToUpdated.Email);
            if (rfa == null)
            {
                // cas d'un changement de role mais avec un utilisateur qui existe déjà

                rfa = Rfa.Create(
                    userToUpdated.Id,
                    new Email(request.User.Email),
                    new PhoneNumber(request.PhoneNumber),
                    new Profession(request.Profession));

                await _repository.Add(rfa);
            }
            else
            {
                rfa = Rfa.Update(rfa, new Email(request.User.Email), new PhoneNumber(request.PhoneNumber), new Profession(request.Profession));

                await _repository.Update(rfa);
            }

            return new Unit();
        }


        public async Task<Unit> Handle(DeleteRfaCommand request, CancellationToken cancellationToken)
        {
            var rfaToRemove = await _repository.GetRfaByEmail(new Email(request.Email));

            if (rfaToRemove == null)
            {
                throw new NotFoundRfaException();
            }

            await _repository.Delete(rfaToRemove);

            return new Unit();
        }
    }
}

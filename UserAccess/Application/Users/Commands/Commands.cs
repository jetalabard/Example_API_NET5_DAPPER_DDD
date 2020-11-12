using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain.Emails;
using MediatR;
using UserAccess.Application.Rfas;
using UserAccess.Application.Users.Outputs;
using UserAccess.Domain.Exceptions;
using UserAccess.Domain.Users;

namespace UserAccess.Application.Users.Commands
{
    public record AddUserCommand(string FirstName, string LastName, string Email, RoleUser Role) : IRequest<UserOutput>;

    public record UpdateUserCommand(Guid Id, string FirstName, string LastName, string Email, RoleUser Role) : IRequest;

    public record RoleUser(string Name, int Permission);

    public record DeleteUserCommand(Guid Id) : IRequest<bool>;

    public record ActivateUserCommand(string UserId, bool Active) : IRequest;

    public class UserCommandHandler : IRequestHandler<AddUserCommand, UserOutput>, IRequestHandler<UpdateUserCommand>, IRequestHandler<DeleteUserCommand, bool>
        , IRequestHandler<ActivateUserCommand>
    {
        private readonly IUserRepository _repository;

        private readonly IRoleRepository _roleRepository;
        private readonly IRfaRepository _rfaRepository;

        public UserCommandHandler(IUserRepository repository, IRoleRepository roleRepository, IRfaRepository rfaRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _rfaRepository = rfaRepository;
        }
        public async Task<UserOutput> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {

            var role = await _roleRepository.GetRole(request.Role.Name);

            if (role == null)
            {
                throw new RoleNotFoundForAddUserException();
            }
            var email = new Email(request.Email);
            var id = new UserId(Guid.NewGuid());
            var existEmail = await _repository.CheckMailExist(id,email);

            if(existEmail)
            {
                throw new EmailAlreadyAffectedException();
            }

            var user = User.Create(
                id,
                new FirstName(request.FirstName), 
                new LastName(request.LastName),
                email,
                role
                );

            await _repository.AddAsync(user);

            return new UserOutput(user.Id.Value, user.LastName.Value, user.FirstName.Value, user.Email.Value, user.IsActive.Value,
                new RoleOutput(user.Role.Name.Value, user.Role.Permissions.Value));
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var id = new UserId(request.Id);
            var userToUpdated = await _repository.GetById(id);
            if (userToUpdated == null)
            {
                throw new UserNotFoundForUpdateException();
            }

            var role = await _roleRepository.GetRole(request.Role.Name);
            if (role == null)
            {
                throw new RoleNotFoundForUpdateUserException();
            }

            var email = new Email(request.Email);
            var existEmail = await _repository.CheckMailExist(id, email);
            
            if (existEmail)
            {
                throw new EmailAlreadyAffectedException();
            }


            if (userToUpdated.Role.Name.Value == "rfa" && userToUpdated.Role.Name.Value != request.Role.Name)
            {
                // cas du changement de role quand l'ancien role est rfa
                var rfaToRemove = await _rfaRepository.GetRfaByEmail(userToUpdated.Email);
                if (rfaToRemove != null)
                {
                    await _rfaRepository.Delete(rfaToRemove);
                }
            }

            userToUpdated.UpdateFirstName(request.FirstName);
            userToUpdated.UpdateLastName(request.LastName);
            userToUpdated.UpdateEmail(request.Email);
            userToUpdated.UpdateRole(role);

            await _repository.Update(userToUpdated);


           

            return new Unit();
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var id = new UserId(request.Id);
            var userToDelete = await _repository.GetById(id);
            if (userToDelete == null)
            {
                throw new UserNotFoundForDeleteException();
            }


            await _repository.Delete(userToDelete);

            return true;
        }

        public async Task<Unit> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var email = new Email(request.UserId);
            var userToActivate = await _repository.GetByEmail(email);
            if (userToActivate == null)
            {
                throw new UserNotFoundForDeleteException();
            }

            userToActivate.UpdateIsActive(request.Active);

            await _repository.Update(userToActivate);

            return new Unit();
        }
    }
}

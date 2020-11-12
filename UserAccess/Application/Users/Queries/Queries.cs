using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Queries;
using MediatR;
using UserAccess.Application.Users.Outputs;

namespace UserAccess.Application.Queries
{
    public record GetUsersQuery : IRequest<IEnumerable<UserOutput>>;

    public record GetUsersByEmailQuery(string UserEmail) : IRequest<UserOutput>;
    
    public record GetRoleQuery(string UserEmail) : IRequest<RoleOutput>;

    public record GetRolesQuery : IRequest<IEnumerable<RoleOutput>>;

    public class UserQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserOutput>>, IRequestHandler<GetUsersByEmailQuery, UserOutput>,
         IRequestHandler<GetRoleQuery, RoleOutput>, IRequestHandler<GetRolesQuery, IEnumerable<RoleOutput>>
    {
        private readonly IDbQueryHelper _dbQueryHelper;
        public UserQueryHandler(IDbQueryHelper dbQueryHelper)
        {
            _dbQueryHelper = dbQueryHelper;
        }

        public async Task<IEnumerable<UserOutput>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return (await _dbQueryHelper.Query<UserRequest>($@"
                                    SELECT u.id, u.firstName, u.lastName, u.email, r.name, r.id as code, r.permissions as permission, u.isActive
                                    FROM [User] u
                                    INNER JOIN [Role] r ON u.RoleId = r.Id"))
                              .Select(user => new UserOutput(user.Id, user.LastName, user.FirstName, user.Email, user.IsActive, 
                              new RoleOutput(user.Name, user.Permission)));
        }

        public async Task<RoleOutput> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            return await _dbQueryHelper.QuerySingle<RoleOutput>($@"SELECT r.Id as Code, r.Name, r.Permissions as Permission 
                                                                    FROM [User] u
                                                                   JOIN [Role] r ON u.RoleId = r.Id
                                                                   WHERE u.Email = @UserEmail and u.isActive = 1", new { request.UserEmail });
        }

        public async Task<IEnumerable<RoleOutput>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return await _dbQueryHelper.Query<RoleOutput>($@"SELECT Id as Code, Name, Permissions as Permission FROM Role");
        }

        public async Task<UserOutput> Handle(GetUsersByEmailQuery request, CancellationToken cancellationToken)
        {
            return (await _dbQueryHelper.Query<UserRequest>($@"
                                    SELECT u.id, u.firstName, u.lastName, u.email, r.name, r.id as code, r.permissions as permission, u.isActive
                                    FROM [User] u
                                    INNER JOIN [Role] r ON u.RoleId = r.Id
									where u.email = @Email", new { Email = request.UserEmail}))
                    .Select(user => new UserOutput(user.Id, user.LastName, user.FirstName, user.Email, user.IsActive,
                    new RoleOutput(user.Name, user.Permission)))
                    .First();
        }
    }
}

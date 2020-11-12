using System.Threading;
using System.Threading.Tasks;
using Common.Application.Queries;
using MediatR;
using Support.Application.Rfas.Outputs;
using System.Collections.Generic;
using System.Linq;
using Common.Domain.Emails;

namespace Support.Application.ApplicationInfos.Queries
{
    public record GetAllRfasQuery : IRequest<IEnumerable<RfaOutput>>;

    public record GetRfaQuery(string Email) : IRequest<RfaOutput>;

    public class RfaQueryHandler : IRequestHandler<GetAllRfasQuery, IEnumerable<RfaOutput>>, IRequestHandler<GetRfaQuery, RfaOutput>
    {
        private readonly IDbQueryHelper _dbQueryHelper;

        public RfaQueryHandler(IDbQueryHelper dbQueryHelper)
        {
            _dbQueryHelper = dbQueryHelper;
        }

        public async Task<IEnumerable<RfaOutput>> Handle(GetAllRfasQuery request, CancellationToken cancellationToken)
        {
            return (await _dbQueryHelper.Query<RfaRequest>(@"
                select 
                rfa.PhoneNumber, rfa.Profession,
                [User].LastName, [User].FirstName, rfa.Email
                from RfaInfo rfa
                inner join [User] on [User].Id = rfa.UserId
                where [User].isActive = 1;
             ")).Select(rfa => new RfaOutput(rfa.PhoneNumber, rfa.Profession, new RfaUserOutput(rfa.LastName, rfa.FirstName, rfa.Email)));
        }

        public async Task<RfaOutput> Handle(GetRfaQuery request, CancellationToken cancellationToken)
        {
            var rfa = (await _dbQueryHelper.QuerySingle<RfaRequest>(@"
                select 
                rfa.PhoneNumber, rfa.Profession,
                [User].LastName, [User].FirstName, rfa.Email
                from RfaInfo rfa
                inner join [User] on [User].Id = rfa.UserId 
                where rfa.email = @Email
             ", new { @Email = new Email(request.Email).Value }));

            if(rfa == null)
            {
                return default;
            }

             return new RfaOutput(rfa.PhoneNumber, rfa.Profession, new RfaUserOutput(rfa.LastName, rfa.FirstName, rfa.Email));
        }
    }
}

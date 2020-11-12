using System.Threading;
using System.Threading.Tasks;
using Support.Application.ApplicationInfos.Outputs;
using MediatR;
using Common.Application.Queries;
using System.Linq;

namespace Support.Application.ApplicationInfos.Queries
{
    public record GetApplicationInfoQuery : IRequest<ApplicationInfoOutput>;

    public class ApplicationInfoQueryHandler : IRequestHandler<GetApplicationInfoQuery, ApplicationInfoOutput>
    {
        private readonly IDbQueryHelper _dbQueryHelper;

        public ApplicationInfoQueryHandler(IDbQueryHelper dbQueryHelper)
        {
            _dbQueryHelper = dbQueryHelper;
        }

        public async Task<ApplicationInfoOutput> Handle(GetApplicationInfoQuery request, CancellationToken cancellationToken)
        {
            return (await _dbQueryHelper.Query<ApplicationInfoOutput>(@"
                select NameApp, VersionApp, CodeApp from ApplicationInfo;
             ")).Last();
        }
    }
}

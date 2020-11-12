using System.Threading.Tasks;
using Support.Application.ApplicationInfos;
using Support.Domain.ApplicationInfos;
using Microsoft.EntityFrameworkCore;

namespace Support.Infrastructure.Repository
{
    public class ApplicationInfoRepository : IApplicationInfoRepository
    {
        private readonly SupportContext _supplierManagementContext;

        public ApplicationInfoRepository(SupportContext supplierManagementContext)
        {
            _supplierManagementContext = supplierManagementContext;
        }

        public async Task<ApplicationInfo> Get()
        {
            return await _supplierManagementContext.ApplicationInfos
                .FirstOrDefaultAsync();
        }
    }
}

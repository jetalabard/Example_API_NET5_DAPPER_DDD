
using Support.Domain.ApplicationInfos;
using System.Threading.Tasks;

namespace Support.Application.ApplicationInfos
{
    public interface IApplicationInfoRepository
    {
        Task<ApplicationInfo> Get();
    }
}

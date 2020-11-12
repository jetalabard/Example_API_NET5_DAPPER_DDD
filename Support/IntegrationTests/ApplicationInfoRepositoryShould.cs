using FluentAssertions;
using Xunit;
using Support.Infrastructure.Repository;
using Support.IntegrationTests.Helpers;
using System.Threading.Tasks;

namespace Support.IntegrationTests
{
    public class ApplicationInfoRepositoryShould : IntegrationTestBaseSupport
    {
        private readonly ApplicationInfoRepository _applicationInfoRepository;

        public ApplicationInfoRepositoryShould() : base()
        {
            _applicationInfoRepository = new ApplicationInfoRepository(ExampleContext);
        }

        [Fact]
        public async Task GetApplicationInfoFromEf()
        {          
            var applicationInfo = await _applicationInfoRepository.Get();

            applicationInfo.Should().NotBeNull();
            applicationInfo.NameApp.ToString().Should().Be("Example");
        }
    }
}

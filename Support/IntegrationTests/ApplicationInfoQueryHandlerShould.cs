using FluentAssertions;
using Xunit;
using Dapper;
using System;
using Common.Infrastructure;
using Support.Application.ApplicationInfos.Queries;
using Support.Infrastructure.Repository;
using Support.IntegrationTests.Helpers;
using System.Threading.Tasks;

namespace Support.IntegrationTests
{
    public class ApplicationInfoQueryHandlerShould : IntegrationTestBaseSupport
    {
        private readonly ApplicationInfoRepository _applicationInfoRepository;
        private readonly ApplicationInfoQueryHandler _applicationInfoQueryHandler;

        public ApplicationInfoQueryHandlerShould() : base()
        {
            var dapperQueryHelper = new DapperQueryHelper(() => Dbconnection);
            _applicationInfoRepository = new ApplicationInfoRepository(ExampleContext);
            _applicationInfoQueryHandler = new ApplicationInfoQueryHandler(dapperQueryHelper);
        }

        [Fact]
        public async Task GetApplicationInfo()
        {
            var applicationInfoId = Guid.NewGuid();
            Dbconnection.Execute("INSERT INTO ApplicationInfo VALUES (@Id,'Example','0.1.0','example');", new { Id = applicationInfoId });

            var result = await _applicationInfoQueryHandler.Handle(new GetApplicationInfoQuery(), default);

            result.Should().NotBeNull();
            result.NameApp.Should().Be("Example");
        }

        [Fact]
        public async Task GetApplicationInfoFromEf()
        {
            var applicationInfoId = Guid.NewGuid();
            Dbconnection.Execute("INSERT INTO ApplicationInfo VALUES (@Id,'Example','0.1.0','example');", new { Id = applicationInfoId });

            var applicationInfo = await _applicationInfoRepository.Get();

            applicationInfo.Should().NotBeNull();
            applicationInfo.NameApp.ToString().Should().Be("Example");
        }
    }
}

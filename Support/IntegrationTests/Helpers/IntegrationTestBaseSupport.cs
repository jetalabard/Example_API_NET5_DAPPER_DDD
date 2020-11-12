using System;
using Common.Tests.IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Support.Infrastructure;

namespace Support.IntegrationTests.Helpers
{
    public abstract class IntegrationTestBaseSupport : IntegrationTestBase<SupportContext>, IDisposable
    {
        private bool disposedValue;

        protected IntegrationTestBaseSupport() : base()
        {
            ExampleContext = new SupportContext(OptionsBuilder.Options);
            ExampleContext.Database.Migrate();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Dbconnection.Dispose();
                    ExampleContext.Dispose();
                    DeleteDatabase();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
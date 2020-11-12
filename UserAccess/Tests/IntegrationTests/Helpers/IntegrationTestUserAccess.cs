using System;
using UserAccess.Infrastructure;
using Common.Tests.IntegrationTests;

namespace UserAccess.IntegrationTests.Helpers
{
    public abstract class IntegrationTestUserAccess : IntegrationTestBase<UserAccessContext>, IDisposable
    {
        private bool disposedValue;

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
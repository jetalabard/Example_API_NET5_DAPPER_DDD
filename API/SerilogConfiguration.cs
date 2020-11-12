using Serilog;
using Serilog.Core;

namespace Example.API
{
    internal static class SerilogConfiguration
    {
        internal static Logger GetLogger()
        {
            return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }
    }
}

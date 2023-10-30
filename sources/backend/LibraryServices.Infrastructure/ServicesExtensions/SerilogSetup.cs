using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class SerilogSetup
    {
        public static void AddSerilogSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", "log"), rollingInterval: RollingInterval.Hour)
                .CreateLogger();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(Log.Logger);
            });
        }
    }
}

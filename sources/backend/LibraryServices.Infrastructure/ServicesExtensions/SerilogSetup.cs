using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Builder;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class SerilogSetup
    {
        public static void AddSerilogSetup(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Filter.ByExcluding("RequestPath like '/health'")
                .Filter.ByExcluding("StartsWith(@m,'requestId: no request id, previousRequestId: no previous request id, message: Started polling')")
                .Filter.ByExcluding("StartsWith(@m,'requestId: no request id, previousRequestId: no previous request id, message: Finished polling')")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", "log"), rollingInterval: RollingInterval.Hour)
                .WriteTo.Seq(configuration["SEQ_URL"]!, apiKey: configuration["SEQ_APIKEY"])
                .CreateLogger();
            builder.Services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(Log.Logger);
            });

            builder.Host.UseSerilog(Log.Logger, true);
        }
    }
}

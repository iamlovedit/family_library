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
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configuration);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Filter.ByExcluding("RequestPath like '/health'")
                .Filter.ByExcluding("StartsWith(@m,'requestId: No RequestId, previousRequestId: No PreviousRequestId, message: ')")
                .Filter.ByExcluding("StartsWith(@m,'requestId: No RequestId, previousRequestId: No PreviousRequestId, message: ')")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", "log"), rollingInterval: RollingInterval.Hour)
                .WriteTo.Seq(configuration["SEQ_URL"]!, apiKey: configuration["SEQ_APIKEY"])
                .CreateLogger();
            builder.Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddSerilog(Log.Logger);
            });

            builder.Host.UseSerilog(Log.Logger, true);
        }
    }
}

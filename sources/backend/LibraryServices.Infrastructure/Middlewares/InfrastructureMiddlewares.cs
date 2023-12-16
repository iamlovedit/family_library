using System.Net;
using LibraryServices.Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace LibraryServices.Infrastructure.Middlewares
{
    public static class InfrastructureMiddlewares
    {
        public static void UseInfrastructure(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseVersionedSwaggerUI();
            }

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    var message = new MessageData<Exception>(false, "An exception was thrown", 500);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                });
            });

            app.MapHealthChecks("health");

            app.UseCors("cors");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.UseSerilogLogging();

            app.Run();
        }

        public static void UseSerilogLogging(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseSerilogRequestLogging(options =>
            {
                // Customize the message template
                options.MessageTemplate = "[{RemoteIpAddress}] [{RequestScheme}] [{RequestHost}] [{RequestMethod}] [{RequestPath}] responded [{StatusCode}] in [{Elapsed:0.0000}] ms";

                // Emit debug-level events instead of the defaults
                // options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("RemoteIpAddress", httpContext.Request.Headers["X-Forwarded-For"].ToString());
                };
            });
        }


        public static void UseInitSeed(this IApplicationBuilder app, Action<DatabaseSeed> seedBuilder)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(seedBuilder);
            using var scope = app.ApplicationServices.CreateScope();
            var databaseSeed = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
            if (databaseSeed == null)
            {
                return;
            }
            seedBuilder.Invoke(databaseSeed);
        }
    }
}

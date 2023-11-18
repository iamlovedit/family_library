using System.Net;
using LibraryServices.Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace LibraryServices.Infrastructure.Middlewares
{
    public static class InfrastructureMiddlewares
    {
        public static void UseInfrastructure(this WebApplication app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

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

            app.Run();
        }
        public static void UseInitSeed(this IApplicationBuilder app, Action<DatabaseSeed> seedBuilder)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (seedBuilder == null)
            {
                throw new ArgumentNullException(nameof(seedBuilder));
            }
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

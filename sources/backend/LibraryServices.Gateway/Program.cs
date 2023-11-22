using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using LibraryServices.Infrastructure.ServicesExtensions;
using Ocelot.Provider.Polly;
using Ocelot.Provider.Consul;
using LibraryServices.Infrastructure.Consul;
using LibraryServices.Infrastructure.Middlewares;
const string _corsName = "cors";
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddOcelot(builder.Configuration).AddConsul().AddConfigStoredInConsul().AddPolly();
builder.AddSerilogSetup();
services.AddHealthChecks();
services.AddConsulSetup(builder.Configuration);

services.AddCors(option =>
{
    option.AddPolicy(_corsName, policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.WebHost.ConfigureAppConfiguration((builderContext, builder) =>
{
    builder.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true, false)
        .AddJsonFile($"ocelot.{builderContext.HostingEnvironment.EnvironmentName}.json", false, false)
        .AddEnvironmentVariables();
});
var app = builder.Build();

app.UseHealthChecks("/health");

app.UseCors(_corsName);

app.UseSerilogLogging();

await app.UseOcelot().ConfigureAwait(true);
app.Run();

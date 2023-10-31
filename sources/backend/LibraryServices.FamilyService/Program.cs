using LibraryServices.FamilyService.Services;
using LibraryServices.Infrastructure.Middlewares;
using LibraryServices.Infrastructure.ServicesExtensions;
using Minio;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddScoped<IFamilyService, FamilyService>();
builder.AddInfrastructureSetup();

services.AddMinio(client =>
{
    client.WithEndpoint("").WithCredentials("", "").WithSSL();
});
var app = builder.Build();

app.UseInfrastructure();
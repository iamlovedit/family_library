using LibraryServices.Domain.Models.FamilyLibrary;
using LibraryServices.FamilyService.Services;
using LibraryServices.Infrastructure.Middlewares;
using LibraryServices.Infrastructure.ServicesExtensions;
using Minio;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration= builder.Configuration;
services.AddScoped<IFamilyService, FamilyService>();
builder.AddInfrastructureSetup();

services.AddMinio(client =>
{
    client.WithEndpoint(configuration["MINIO_HOST"]).WithCredentials(configuration["MINIO_ROOT_USER"], configuration["MINIO_ROOT_PASSWORD"]);
});
var app = builder.Build();

app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass<Family>();
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }
    var pathFormat = Path.Combine(app.Environment.WebRootPath, "Seed", "{0}.json");
    var file = string.Format(pathFormat, "Families");
    dbSeed.InitSeed<Family>(file);

    file = string.Format(pathFormat, "FamilySymbols");
    dbSeed.InitSeed<FamilySymbol>(file);

    file = string.Format(pathFormat, "FamilyCategories");
    dbSeed.InitSeed<FamilyCategory>(file);
});

app.UseInfrastructure();
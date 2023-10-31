using LibraryServices.Domain.Models.Dynamo;
using LibraryServices.Infrastructure.Middlewares;
using LibraryServices.Infrastructure.ServicesExtensions;
using LibraryServices.PackageService.Jobs;
using LibraryServices.PackageService.Services;
using Quartz;
using Quartz.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IPackageService, PackageService>();
services.AddScoped<IVersionService, VersionService>();
builder.AddInfrastructureSetup();

services.AddQuartz(options =>
{
    var jobKey = new JobKey("update packages");
    options.AddJob<FetchPackagesJob>(config => config.WithIdentity(jobKey));
    options.AddTrigger(config =>
    {
        config.ForJob(jobKey)
            .WithIdentity("update packages")
            .WithCronSchedule("0 0 5 1/1 * ? *");
    });
});

services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = true;
});

services.AddHttpClient();
var app = builder.Build();

app.UseInitSeed(seed =>
{
    seed.InitTablesByClass<Package>();
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }
    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Packages");
    seed.InitSeed<Package>(file);

    file = string.Format(seedFolder, "PackageVersions");
    seed.InitSeed<PackageVersion>(file);
});

app.UseInfrastructure();

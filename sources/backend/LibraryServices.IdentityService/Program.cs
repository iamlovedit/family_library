using LibraryServices.Domain.Models.Identity;
using LibraryServices.IdentityService.Services;
using LibraryServices.Infrastructure.Middlewares;
using LibraryServices.Infrastructure.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddScoped<IUserService, UserService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<IUserRoleService, UserRoleService>();

builder.AddInfrastructureSetup();


var app = builder.Build();
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass<User>();

    var pathFormat = Path.Combine(app.Environment.WebRootPath, "Seed", "{0}.json");
    var jsonFile = string.Format(pathFormat, "Users");
    dbSeed.InitSeed<User>(jsonFile);

    jsonFile = string.Format(pathFormat, "Roles");
    dbSeed.InitSeed<Role>(jsonFile);

    jsonFile = string.Format(pathFormat, "UserRoles");
    dbSeed.InitSeed<UserRole>(jsonFile);
});
app.UseInfrastructure();

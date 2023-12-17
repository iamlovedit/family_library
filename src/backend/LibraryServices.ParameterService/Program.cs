using LibraryServices.Infrastructure.Middlewares;
using LibraryServices.Infrastructure.ServicesExtensions;
using LibraryServices.ParameterService.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddScoped<IParameterService, ParameterService>();
services.AddScoped<IParameterDefinitionService, ParameterDefinitionService>();
builder.AddInfrastructureSetup();


var app = builder.Build();

app.UseInfrastructure();

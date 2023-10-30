using LibraryServices.Infrastructure.Middlewares;
using LibraryServices.Infrastructure.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddInfrastructureSetup();


var app = builder.Build();

app.UseInfrastructure();

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddOcelot(builder.Environment);
builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();
app.Run();

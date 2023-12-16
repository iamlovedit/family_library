using LibraryServices.Infrastructure.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class DatabaseSeedSetup
    {
        public static void AddDatabaseSeedSetup(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<DatabaseContext>();
            services.AddScoped<DatabaseSeed>();
        }
    }
}

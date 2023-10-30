using LibraryServices.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SqlSugar;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (hostEnvironment is null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            SnowFlakeSingle.WorkId = 1;
            var connectionString =
              $"server={configuration["POSTGRESQL_HOST"]};" +
              $"port={configuration["POSTGRESQL_PORT"]};" +
              $"database={configuration["POSTGRESQL_DATABASE"]};" +
              $"userid={configuration["POSTGRESQL_USER"]};" +
              $"password={configuration["POSTGRESQL_PASSWORD"]};";

            var connectionConfig = new ConnectionConfig()
            {
                DbType = DbType.PostgreSQL,
                ConnectionString = connectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    PgSqlIsAutoToLower = false,
                    PgSqlIsAutoToLowerCodeFirst = false,
                }
            };

            var sugarScope = new SqlSugarScope(connectionConfig, config =>
            {
                config.QueryFilter.AddTableFilter<IDeletable>(d => !d.IsDeleted);
                if (hostEnvironment.IsDevelopment() || hostEnvironment.IsStaging())
                {
                    config.Aop.OnLogExecuting = (sql, parameters) =>
                    {
                        Log.Logger.Information(sql);
                    };
                }
            });

            services.AddSingleton<ISqlSugarClient>(sugarScope);
        }
    }
}

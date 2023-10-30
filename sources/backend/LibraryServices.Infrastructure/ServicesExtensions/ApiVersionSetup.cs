using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class ApiVersionSetup
    {
        public static void AddApiVersionSetup(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("library-api-version"),
                    new MediaTypeApiVersionReader("library-api-version"));
            }).AddApiExplorer(builder =>
            {
                builder.GroupNameFormat = "'v'VVV";
                builder.SubstituteApiVersionInUrl = true;
            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();
        }
    }
}

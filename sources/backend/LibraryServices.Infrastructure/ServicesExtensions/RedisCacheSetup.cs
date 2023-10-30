using LibraryServices.Infrastructure.RedisCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IRedisBasketRepository, RedisBasketRepository>();
            services.AddSingleton(provider => new RedisRequirement(TimeSpan.FromMinutes(30)));
            services.AddSingleton(provider =>
            {
                var redisConnectionString = $"{configuration["REDIS_HOST"]},password={configuration["REDIS_PASSWORD"]}";
                var redisConfig = ConfigurationOptions.Parse(redisConnectionString, true);
                redisConfig.ResolveDns = true;
                return ConnectionMultiplexer.Connect(redisConfig);
            });
        }
    }
}

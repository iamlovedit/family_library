using LibraryServices.EventBus.EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SqlSugar.Extensions;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class RabbitMQSetup
    {
        public static void AddRabbitMQSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RABBITMQ_HOST"],
                    UserName = configuration["RABBITMQ_USERNAME"],
                    Password = configuration["RABBITMQ_PASSWORD"],
                    Port = configuration["RBBITMQ_PORT"]?.ObjToInt()
                    ?? throw new ArgumentNullException("port is null"),
                    DispatchConsumersAsync = true
                };
                return new RabbitMQPersistentConnection(factory, logger);
            });
        }
    }
}

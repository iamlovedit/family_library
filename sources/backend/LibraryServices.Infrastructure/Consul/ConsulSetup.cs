using Consul;
using Consul.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryServices.Infrastructure.Consul
{
    public static class ConsulSetup
    {
        public static void AddConsulSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var consulOption = configuration.GetSection("Consul").Get<ConsulOption>();

            services.AddConsul(options =>
            {
                //consul client address
                options.Address = new Uri($"{consulOption!.ConsulAddress}:{consulOption.ConsulPort}");
            });

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(10),
                HTTP = $"http://{consulOption!.IP}:{consulOption.Port}/health", //servcie health check url
                Timeout = TimeSpan.FromSeconds(5)
            };
            services.AddConsulServiceRegistration(option =>
            {
                option.Check = httpCheck;
                option.ID = Guid.NewGuid().ToString();
                option.Name = consulOption.Name;
                option.Address = consulOption.IP;
                option.Port = consulOption.Port;
            });
        }
    }
}

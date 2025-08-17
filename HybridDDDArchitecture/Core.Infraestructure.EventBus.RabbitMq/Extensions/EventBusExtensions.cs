using Core.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
    
namespace Core.Infraestructure
{
    public static class EventBusExtensions
    {
        public static
#nullable disable
        IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqEventBus"));
            string queueName = services.ConfigureConnectionFactory(configuration);
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddSingleton<IEventBusSubscriptionManager, EventBusSubscriptionManager>();
            services.AddSingleton<IEventBus, RabbitMqEventBus>();
            return services;
        }

        private static string ConfigureConnectionFactory(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            RabbitMqOptions sectionData = configuration.GetSectionData<RabbitMqOptions>("RabbitMqEventBus");
            ConnectionFactory implementationInstance;
            if (sectionData.ConnectionString == null)
                implementationInstance = new ConnectionFactory()
                {
                    HostName = sectionData.Host,
                    VirtualHost = sectionData.VirtualHost,
                    UserName = sectionData.UserName,
                    Password = sectionData.Password,
                    Port = sectionData.Port,
                    DispatchConsumersAsync = true
                };
            else
                implementationInstance = new ConnectionFactory()
                {
                    Uri = new Uri(sectionData.ConnectionString),
                    DispatchConsumersAsync = true
                };
            services.AddSingleton<IConnectionFactory>((IConnectionFactory)implementationInstance);
            return sectionData.QueueName;
        }
    }
}

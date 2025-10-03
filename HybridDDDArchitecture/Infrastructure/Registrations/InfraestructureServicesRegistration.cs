using Application.Repositories;
using Core.Application;
using Core.Infraestructure; // Este using ya incluye RabbitMqEventBus y Constants
using Domain.Others.Utils;
using Infrastructure.Constants;
using Infrastructure.Factories;
using Infrastructure.Repositories.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Domain.Enums.Enums;

// **QUITAMOS el using Core.Infraestructure.EventBus.RabbitMq;** // **ya que el namespace real es Core.Infraestructure**

namespace Infrastructure.Registrations
{
    public static class InfraestructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            /* Database Context */
            services.AddRepositories(configuration);

            /* EventBus */
            // El método AddEventBus debe estar definido como un método de extensión 
            // en la clase RabbitMqEventBus o en una clase de registro en el namespace Core.Infraestructure.
            services.AddEventBus(configuration);

            // CORRECCIÓN FINAL DE DI: Registramos el IEventBus con su implementación concreta.
            // Esto es crucial si IIntegrationEventHandler<T> está pidiendo IEventBus.
            services.AddSingleton<Core.Application.IEventBus, Core.Infraestructure.RabbitMqEventBus>();

            // Dado que el error es 'IIntegrationEventHandler<T>', y usted no tiene el archivo 
            // 'RabbitMqIntegrationEventHandler.cs', su proyecto debe usar IEventBus o una clase 
            // diferente para el manejo genérico. 
            // Si el error de compilación persiste, su proyecto espera una implementación de 
            // IIntegrationEventHandler<T> que falta.

            /* Adapters */
            services.AddSingleton<IExternalApiClient, ExternalApiHttpAdapter>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string dbType = configuration["Configurations:UseDatabase" ?? throw new NullReferenceException(InfrastructureConstants.DATABASE_TYPE_NOT_CONFIGURED)];

            services.AddScoped<IAutomovilRepository, AutomovilRepository>();

            services.CreateDataBase(dbType, configuration);

            return services;
        }
    }
}
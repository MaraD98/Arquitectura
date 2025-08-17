using Application.ApplicationServices;
using Core.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Registrations
{
    /// <summary>
    /// Aqui se deben registrar todas las dependencias de la capa de aplicacion
    /// </summary>
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            /* Automapper */
            services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

            /* EventBus */
            services.AddPublishers();
            services.AddSubscribers();

            /* MediatR*/
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();

            /* Application Services */
            services.AddScoped<IDummyEntityApplicationService, DummyEntityApplicationService>();

            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            //Aqui se registran los handlers que publican en el bus de eventos
            services.AddTransient<IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>, DummyEntityCreatedIntegrationEventHandlerPub>();
            return services;
        }

        private static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            //Aqui se registran los handlers que se suscriben al bus de eventos
            services.AddTransient<DummyEntityCreatedIntegrationEventHandlerSub>();
            return services;
        }
    }
}

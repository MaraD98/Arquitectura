using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Core.Application.ComandQueryBus.Buses;
using Application.ApplicationServices;

using AutoMapper;
using Core.Application.EventBus;

namespace Application.Registrations
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
        
            services.AddAutoMapper(cfg =>
            {
                // Utiliza typeof(...).Assembly para asegurar que AutoMapper encuentre los perfiles.
                cfg.AddMaps(typeof(ApplicationServicesRegistration).Assembly);
            });

            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();

            
            services.AddScoped<IDummyEntityApplicationService, DummyEntityApplicationService>();
            services.AddScoped<IAutomovilApplicationService, AutomovilApplicationService>();

            // 🛑 REGISTRO FALTANTE: Resuelve el error de dependencia de IIntegrationEventHandler<T>
            services.AddIntegrationEventHandlers();

            return services;
        }
    }
}
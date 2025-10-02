// Archivo: Application\Registrations\ApplicationServicesRegistration.cs

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// CORRECCIÓN CS0246: Agregamos el using al namespace completo y correcto del Bus
using Core.Application.ComandQueryBus.Buses;

namespace Application.Registrations
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrar MediatR (asumiendo que está aquí)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Se registra la interfaz ICommandQueryBus con su implementación
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();

            return services;
        }
    }
}

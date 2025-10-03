using Microsoft.Extensions.DependencyInjection; // <-- Ahora disponible gracias al NuGet
using System.Reflection;
using System.Linq;

namespace Core.Application.EventBus
{
    public static class EventBusRegistrationExtensions
    {
        // ... (cuerpo del método AddIntegrationEventHandlers)
        public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var applicationAssembly = Assembly.Load("Application");
            var handlerType = typeof(IIntegrationEventHandler<>);

            var handlers = applicationAssembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                .ToList();

            foreach (var handler in handlers)
            {
                var implementedInterface = handler.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);

                services.AddTransient(implementedInterface, handler);
            }

            return services;
        }
    }
}
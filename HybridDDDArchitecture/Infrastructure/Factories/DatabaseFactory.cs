using Application.Repositories;
using Domain.Others.Utils;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using System;
using static Domain.Enums.Enums;

namespace Infrastructure.Factories
{
    internal static class DatabaseFactory
    {
        public static void CreateDataBase(this IServiceCollection services, string dbType, IConfiguration configuration)
        {
            switch (dbType.ToEnum<DatabaseType>())
            {
                case DatabaseType.MYSQL:
                case DatabaseType.MARIADB:
                case DatabaseType.SQLSERVER:
                    services.AddSqlServerRepositories(configuration);
                    // 🚨 Se ejecuta la aplicación de migraciones después del registro del contexto
                    services.ApplyMigrations();
                    break;
                case DatabaseType.MONGODB:
                    services.AddMongoDbRepositories(configuration);
                    break;
                default:
                    throw new NotSupportedException(InfrastructureConstants.DATABASE_TYPE_NOT_SUPPORTED);
            }
        }

        private static IServiceCollection AddSqlServerRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // 🚨 CORRECCIÓN 1: La cadena de conexión en appsettings.json se llama "DefaultConnection"
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no se encontró o está vacía.");
            }

            services.AddDbContext<Repositories.Sql.StoreDbContext>(options =>
            {
                // 🚨 CORRECCIÓN 2: Usar la variable connectionString obtenida arriba
                options.UseSqlServer(connectionString);
            }, ServiceLifetime.Scoped);

            // 🚨 Se elimina el código de Migraciones de aquí.
            // Se moverá a un método de extensión más seguro para evitar problemas de ServiceProvider.

            /* Sql Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Sql.DummyEntityRepository>();
            services.AddTransient<IAutomovilRepository, Repositories.Sql.AutomovilRepository>();

            return services;
        }

        // 🚨 MÉTODO AGREGADO: Aplica migraciones de forma segura
        private static void ApplyMigrations(this IServiceCollection services)
        {
            // Usamos BuildServiceProvider().CreateScope() para crear un ámbito temporal
            // que nos permite obtener el DbContext sin interferir con el ciclo de vida del Host.
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Repositories.Sql.StoreDbContext>();

                // 🚨 La migración se aplica aquí de forma segura
                context.Database.Migrate();
            }
        }

        private static IServiceCollection AddMongoDbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            Repositories.Mongo.StoreDbContext db = new(configuration.GetConnectionString("MongoConnection") ?? throw new NullReferenceException());
            services.AddSingleton(typeof(Repositories.Mongo.StoreDbContext), db);

            /* MongoDb Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Mongo.DummyEntityRepository>();

            return services;
        }
    }
}
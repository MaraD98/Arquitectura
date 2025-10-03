using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; // Necesario para IConfiguration y ConfigurationBuilder
using System.IO; // Necesario para Path y Directory
using System;
using System.Reflection; // Necesario para GetTypeInfo().Assembly

// El namespace debe coincidir con el de tu DbContext.
namespace Infrastructure.Repositories.Sql
{
    public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Usamos la cualificación completa para evitar cualquier confusión de tipos
            Microsoft.Extensions.Configuration.IConfigurationBuilder builder =
                new Microsoft.Extensions.Configuration.ConfigurationBuilder();

            // 🛠️ MODIFICACIÓN CLAVE: Aplicamos un CAST explícito en la primera llamada para resolver el error CS1929.
            builder = (Microsoft.Extensions.Configuration.IConfigurationBuilder)builder.SetBasePath(Path.Combine(basePath, "..", "Template-API"));

            // El resto de llamadas de extensión ahora se resuelven correctamente:
            builder = builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder = builder.AddJsonFile($"appsettings.{env}.json", optional: true);

            IConfigurationRoot configuration = builder.Build();

            // -----------------------------------------------------------

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no se encuentra en la configuración.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();

            // Esto asegura que Entity Framework sepa dónde guardar los archivos de migración
            var migrationsAssemblyName = typeof(StoreDbContext).GetTypeInfo().Assembly.GetName().Name;

            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(migrationsAssemblyName);
            });

            return new StoreDbContext(optionsBuilder.Options);
        }
    }
}
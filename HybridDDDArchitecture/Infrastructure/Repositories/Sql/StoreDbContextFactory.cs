using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure.Repositories.Sql
{
    // 1. Implementación de la Interfaz: Esto le indica a 'dotnet ef' que aquí está la receta.
    public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            // 2. Cargar la Configuración: 
            // Carga manualmente appsettings.json y appsettings.Development.json 
            // desde la ubicación del proyecto de inicio (Template-API).
            var basePath = Directory.GetCurrentDirectory();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(basePath, "..", "Template-API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();

            // 3. Obtener la Cadena de Conexión: Lee la cadena de conexión de Docker.
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no se encuentra en la configuración.");
            }

            // 4. Construir Opciones: Prepara las opciones necesarias para configurar SQL Server.
            var builder = new DbContextOptionsBuilder<StoreDbContext>();
            builder.UseSqlServer(connectionString);

            // 5. Instanciar y Retornar: Crea una nueva instancia de tu StoreDbContext usando las opciones configuradas.
            return new StoreDbContext(builder.Options);
        }
    }
}

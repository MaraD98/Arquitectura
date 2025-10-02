using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
// 🚨 ELIMINAMOS los usings específicos (Json y FileExtensions) para resolver el CS0234
// Se asume que los métodos de extensión son visibles a través del PackageReference.

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

            // La sintaxis de ruptura de cadena con métodos de extensión es correcta y debe funcionar.
            builder = builder.SetBasePath(Path.Combine(basePath, "..", "Template-API"));
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
            optionsBuilder.UseSqlServer(connectionString);

            return new StoreDbContext(optionsBuilder.Options);
        }
    }
}
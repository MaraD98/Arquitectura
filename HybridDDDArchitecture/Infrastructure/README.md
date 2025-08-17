## Configurar para usar Entity Framework Core con SQL Server

Para configurar Entity Framework Core con SQL Server en el proyecto, sigue estos pasos:

1. Crear una base de datos en SQL Server. Puedes usar SQL Server Management Studio 
o cualquier otra herramienta para crear una base de datos llamada `HybridDDDArchitecture`.

2. Copiar la cadena de conexión a la base de datos y agregarla al archivo `appsettings.json` 
del proyecto `Template-API`:

"ConnectionStrings":{
  "SqlConnection": "Server=localhost;Database=HybridDDDArchitecture;User Id=sa;Password=your_password;"
}

## Migraciones

3. Crear la primera migracion de la base de datos, ejecutando el siguiente comando:

dotnet ef migrations add Initial --project .\Infrastructure.csproj --startup-project ..\Template-API\Template-API.csproj

4. Ejecutar el proyecto, esto actualizará la base de datos con la migración creada, y estará en
condiciones de usarse.

5. La tabla ´__EFMigrationsHistory´ es creada por Entity Framework Core para llevar el control de las migraciones aplicadas 
1. a la base de datos.
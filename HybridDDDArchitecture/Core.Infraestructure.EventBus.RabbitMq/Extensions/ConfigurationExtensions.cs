using Microsoft.Extensions.Configuration;

namespace Core.Infraestructure
{
    public static class ConfigurationExtensions
    {
        public static TModel GetSectionData<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            TModel instance = new();
            configuration.GetSection(section).Bind(instance);
            return instance;
        }
    }
}

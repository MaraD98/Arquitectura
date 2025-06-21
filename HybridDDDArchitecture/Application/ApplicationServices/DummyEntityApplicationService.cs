using Application.Repositories;

namespace Application.ApplicationServices
{
    /// <summary>
    /// Ejemplo de un servicio de aplicacion para resolver procesos
    /// relacionados a la entidad Dummy que no son responsabilidad del
    /// handler que ejecuta el caso de uso.
    /// </summary>
    public class DummyEntityApplicationService(IDummyEntityRepository context) : IDummyEntityApplicationService
    {
        private readonly IDummyEntityRepository _context = context ?? throw new ArgumentNullException(nameof(context));

        public bool DummyEntityExist(object value)
        {
            value = value ?? throw new ArgumentNullException(nameof(value));

            var response = _context.FindOne(value);

            return response != null;
        }
    }
}

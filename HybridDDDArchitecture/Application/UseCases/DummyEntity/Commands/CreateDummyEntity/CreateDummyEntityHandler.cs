using Application.ApplicationServices;
using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using Core.Application.ComandQueryBus.Buses;
using System; // Necesario para Exception
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.DummyEntity.Commands.CreateDummyEntity
{
    /// <summary>
    /// Ejemplo de handler que responde al comando <see cref="CreateDummyEntityCommand"/>
    /// y ejecuta el proceso para el caso de uso en cuestion.
    /// Todo handler debe implementar la interfaz <see cref="IRequestCommandHandler{TRequest, TResponse}"/>
    /// si devuelve una respuesta donde <c TRequest> es del tipo <see cref="CreateDummyEntityCommand"/>
    /// y <c TResponse> del tipo de dato definido para la respuesta
    /// /// </summary>
    internal sealed class CreateDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository, IDummyEntityApplicationService dummyEntityApplicationService)
        : IRequestCommandHandler<CreateDummyEntityCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus;
        private readonly IDummyEntityRepository _context = dummyEntityRepository;
        private readonly IDummyEntityApplicationService _dummyEntityApplicationService = dummyEntityApplicationService;

        public async Task<string> Handle(CreateDummyEntityCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.DummyEntity entity = new(request.dummyPropertyOne, request.dummyPropertyTwo);

            if (!entity.IsValid) throw new InvalidEntityDataException(entity.GetErrors());

            // 🚨 CS1061 RESUELTO: El método ahora existe en la interfaz.
            if (await _dummyEntityApplicationService.DummyEntityExistAsync(entity.Id.ToString())) throw new EntityDoesExistException();

            try
            {
                object createdId = await _context.AddAsync(entity);

                await _domainBus.Publish(entity.To<DummyEntityCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException ?? ex);
            }
        }
    }
}
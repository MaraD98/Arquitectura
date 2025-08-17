using Core.Domain.Validators;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    /// <summary>
    /// Ejemplo de validador de entidad Dummy
    /// Todo validador de entidad de dominio debe heredar de <see cref="EntityValidator{TEntity}"/>
    /// Donde TEntity es del tipo <see cref="Core.Domain.Entities.DomainEntity{TEntity, TValidator}"/>
    /// </summary>
    public class DummyEntityValidator : EntityValidator<DummyEntity>
    {
        public DummyEntityValidator()
        {
            //Las reglas de negocio deben ir definidas aca
            RuleFor(x => x.DummyPropertyOne).NotNull().NotEmpty().WithMessage(DomainConstants.NOTNULL_OR_EMPTY);
        }
    }
}

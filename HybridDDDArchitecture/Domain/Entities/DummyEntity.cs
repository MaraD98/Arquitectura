using Core.Domain.Entities;
using Domain.Validators;
using static Domain.Enums.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// Ejemplo de entidad de dominio Dummy
    /// Toda entidad de dominio debe heredar de <see cref="DomainEntity{TEntity, TValidator}"/>
    /// Donde T es del tipo <see cref="Core.Domain.Validators.EntityValidator{TEntity}"/>
    /// </summary>
    public class DummyEntity : DomainEntity<int, DummyEntityValidator>
    {
        /// <summary>
        /// Las propiedades de una entidad de dominio deben tener el setter privado. Esto restringe modificaciones
        /// En la capa de Aplicacion y que no es responsabilidad de los objetos de esa capa
        /// </summary>
        public string DummyPropertyTwo { get; private set; }
        public DummyValues DummyPropertyThree { get; private set; }

        public DummyEntity()
        {
        }

        public DummyEntity(string dummyPropertyTwo, DummyValues dummyPropertyThree)
        {
            SetDummyPropertyTwo(dummyPropertyTwo);
            DummyPropertyThree = dummyPropertyThree;
        }

        public DummyEntity(int dummyIdProperty, string dummyPropertyTwo, DummyValues dummyPropertyThree)
        {
            Id = dummyIdProperty;
            SetDummyPropertyTwo(dummyPropertyTwo);
            DummyPropertyThree = dummyPropertyThree;
        }

        public void SetDummyPropertyTwo(string value)
        {
            DummyPropertyTwo = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void SetDummyPropertyThree(DummyValues value)
        {
            DummyPropertyThree = value;
        }
    }
}

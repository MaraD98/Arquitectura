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
    public class DummyEntity : DomainEntity<string, DummyEntityValidator>
    {
        /// <summary>
        /// Las propiedades de una entidad de dominio deben tener el setter privado. Esto restringe modificaciones
        /// En la capa de Aplicacion y que no es responsabilidad de los objetos de esa capa
        /// </summary>
        public string DummyPropertyOne { get; private set; }
        public DummyValues DummyPropertyTwo { get; private set; }

        public DummyEntity()
        {
        }

        public DummyEntity(string dummyPropertyOne, DummyValues dummyPropertyTwo)
        {
            Id = Guid.NewGuid().ToString();
            SetdummyPropertyOne(dummyPropertyOne);
            DummyPropertyTwo = dummyPropertyTwo;
        }

        public DummyEntity(string dummyIdProperty, string dummyPropertyOne, DummyValues dummyPropertyTwo)
        {
            Id = dummyIdProperty;
            SetdummyPropertyOne(dummyPropertyOne);
            DummyPropertyTwo = dummyPropertyTwo;
        }

        public void SetdummyPropertyOne(string value)
        {
            DummyPropertyOne = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void SetdummyPropertyTwo(DummyValues value)
        {
            DummyPropertyTwo = value;
        }
    }
}

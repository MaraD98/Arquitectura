using Core.Application;
using static Domain.Enums.Enums;

namespace Application.DomainEvents
{
    /// <summary>
    /// Ejemplo de un evento de dominio para la entidad Dummy.
    /// Todo evento de dominio debe heredar de <see cref="DomainEvent"/>
    /// </summary>
    internal sealed class DummyEntityCreated : DomainEvent
    {
        //Aqui se definen las propiedades compartidas en el evento
        public string Id { get; set; }
        public string DummyPropertyOne { get; set; }
        public DummyValues DummyPropertyTwo { get; set; }
    }
}

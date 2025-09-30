using Core.Application;

namespace Application.DomainEvents
{
    internal class AutomovilActualizado : DomainEvent
    {
        public int AutomovilId { get; set; } //podria dejar solo este
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int? Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
    }
}
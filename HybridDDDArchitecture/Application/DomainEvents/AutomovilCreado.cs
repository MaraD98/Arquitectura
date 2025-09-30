using Core.Application;

namespace Application.DomainEvents
{
    internal class AutomovilCreado : DomainEvent
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public string Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
    }
}

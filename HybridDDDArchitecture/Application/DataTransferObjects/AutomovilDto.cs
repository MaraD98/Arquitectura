using static Domain.Enums.Enums;

namespace Application.DataTransferObjects
{
    public class AutomovilDto
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public string Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
    }
}
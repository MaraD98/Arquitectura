using Core.Application;
using System;
using System.Collections.Generic;


namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    public class UpdateAutomovilCommand : IRequestCommand
    {
        public string Marca{ get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int? Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
        public UpdateAutomovilCommand()
        {
        }
    }
}
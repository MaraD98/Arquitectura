using Core.Domain.Entities;
using Domain.Validators;
using Domain.Others.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Domain.Entities
{
    public class Automovil : DomainEntity<int, AutomovilValidator>
    {
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public string Color { get; private set; }
        public int Fabricacion { get; private set; }
        public string NumeroMotor { get; private set; }
        public string NumeroChasis { get; private set; }
        protected Automovil()
        {
        }
        public Automovil(string marca, string modelo, string color)
        {
            Marca = marca;
            Modelo = modelo;
            Color = color;
            NumeroMotor = GenerarNumeroMotor(marca, modelo);
            NumeroChasis = GenerarNumeroChasis(modelo, color);
            Console.WriteLine($"[Automovil] Marca: {marca}");
            Console.WriteLine($"[Automovil] Modelo: {modelo}");
            Console.WriteLine($"[Automovil] Color: {color}");
            Console.WriteLine($"[Automovil] NumeroMotor generado: {NumeroMotor}");
            Console.WriteLine($"[Automovil] NumeroChasis generado: {NumeroChasis}");
        }
        private string GenerarNumeroMotor(string marca, string modelo)
        {
            var sufijo = Guid.NewGuid().ToString("N").Substring(0, 6);
            return $"MTR-{marca.Substring(0, 3).ToUpper()}-{modelo.Substring(0,
           3).ToUpper()}-{DateTime.Now:yyyyMMddHHmmss}-{sufijo}";
        }
        private string GenerarNumeroChasis(string modelo, string color)
        {
            var sufijo = Guid.NewGuid().ToString("N").Substring(0, 6);
            return $"CHS-{modelo.Substring(0, 3).ToUpper()}-{color.Substring(0,
           3).ToUpper()}-{DateTime.Now:yyyyMMddHHmmss}-{sufijo}";
        }
    }

}

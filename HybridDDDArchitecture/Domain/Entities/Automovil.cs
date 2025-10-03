using Core.Domain;
using Core.Domain.Entities;
using Domain.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Automovil : DomainEntity<int, AutomovilValidator>
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }

        // Constructor para Entity Framework
        private Automovil() { }

        public Automovil(string marca, string modelo, string color)
        {
            Marca = marca;
            Modelo = modelo;
            Color = color;
            Fabricacion = GenerarAñoFabricacion();
            NumeroMotor = GenerarNumeroMotor(modelo, color);
            NumeroChasis = GenerarNumeroChasis(marca, modelo);
            base.Validate(); // 🚨 CORRECCIÓN CS1501: Se llama sin argumentos
        }

        public void UpdateProperties(string color)
        {
            Color = color;
            base.Validate(); // 🚨 CORRECCIÓN CS1501: Se llama sin argumentos
        }

        private static int GenerarAñoFabricacion()
        {
            var añoActual = DateTime.Now.Year;
            var añoMinimo = 1995;
            var random = new Random();
            return random.Next(añoMinimo, añoActual + 1);
        }

        private static string GenerarNumeroMotor(string modelo, string color)
        {
            var modeloCod = modelo.Length >= 3 ? modelo[..3].ToUpper() : modelo.ToUpper().PadRight(3, 'X');
            var colorCod = color.Length >= 3 ? color[..3].ToUpper() : color.ToUpper().PadRight(3, 'X');
            var fechaCod = DateTime.Now.ToString("yyMMdd");
            var random = new Random();
            var sufijo = random.Next(1000, 9999).ToString();

            return $"MTR-{modeloCod}{colorCod}-{fechaCod}-{sufijo}";
        }

        private static string GenerarNumeroChasis(string marca, string modelo)
        {
            var marcaCod = marca.Length >= 3 ? marca[..3].ToUpper() : marca.ToUpper().PadRight(3, 'X');
            var modeloCod = modelo.Length >= 3 ? modelo[..3].ToUpper() : modelo.ToUpper().PadRight(3, 'X');
            var fechaCod = DateTime.Now.ToString("yyMMdd");
            var hash = Guid.NewGuid().ToString()[..4].ToUpper();

            return $"CHS-{marcaCod}{modeloCod}-{fechaCod}-{hash}";
        }
    }
}
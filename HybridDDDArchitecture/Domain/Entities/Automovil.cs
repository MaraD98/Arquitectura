using Core.Domain.Entities;
using Domain.Validators;

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
            Fabricacion = GenerarAñoFabricacion();
            NumeroMotor = GenerarNumeroMotor(modelo,color);
            NumeroChasis = GenerarNumeroChasis(marca, modelo);
        }
        private int GenerarAñoFabricacion()
        {
            var añoActual = DateTime.Now.Year;
            var añoMinimo = 1995;

            var random = new Random();
            return random.Next(añoMinimo, añoActual + 1); 
        }

        private string GenerarNumeroMotor(string modelo, string color)
        {
            var modeloCod = modelo.Length >= 3 ? modelo.Substring(0, 3).ToUpper() : modelo.ToUpper().PadRight(3, 'X');
            var colorCod = color.Length >= 3 ? color.Substring(0, 3).ToUpper() : color.ToUpper().PadRight(3, 'X');

            var fechaCod = DateTime.Now.ToString("yyMMddHHmm"); 
            var sufijo = Guid.NewGuid().ToString("N").Substring(0, 4); 

            return $"MTR-{modeloCod}{colorCod}-{fechaCod}-{sufijo}";
        }
        private string GenerarNumeroChasis(string marca, string modelo)
        {
            var marcaCod = marca.Length >= 3 ? marca.Substring(0, 3).ToUpper() : marca.ToUpper().PadRight(3, 'X');
            var modeloCod = modelo.Length >= 3 ? modelo.Substring(0, 3).ToUpper() : modelo.ToUpper().PadRight(3, 'X');

            var fechaCod = DateTime.Now.ToString("yyMMddHHmm"); 
            var hash = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", "").Replace("+", "").Replace("/", "")
                .Substring(0, 4); 

            return $"CHS-{marcaCod}{modeloCod}-{fechaCod}-{hash}";
        }

        public List<string> Actualizar(
        string marca,
        string modelo,
        string color,
        int? fabricacion,
        string numeroMotor)
        {
            var camposActualizados = new List<string>();

            if (!string.IsNullOrWhiteSpace(marca) && marca != Marca)
            {
                Marca = marca;
                camposActualizados.Add(nameof(Marca));
            }

            if (!string.IsNullOrWhiteSpace(modelo) && modelo != Modelo)
            {
                Modelo = modelo;
                camposActualizados.Add(nameof(Modelo));
            }

            if (!string.IsNullOrWhiteSpace(color) && color != Color)
            {
                Color = color;
                camposActualizados.Add(nameof(Color));
            }

            if (fabricacion is not null && fabricacion >= 1995 && fabricacion <= DateTime.Now.Year && fabricacion != Fabricacion)
            {
                Fabricacion = fabricacion.Value;
                camposActualizados.Add(nameof(Fabricacion));
            }

            if (!string.IsNullOrWhiteSpace(numeroMotor) && numeroMotor != NumeroMotor)
            {
                NumeroMotor = numeroMotor;
                camposActualizados.Add(nameof(NumeroMotor));
            }


            return camposActualizados;
        }

        public bool EsNumeroMotorValido(string numeroMotor)
        {
            return numeroMotor.StartsWith("MTR-") && numeroMotor.Length >= 8;
        }
    }
}

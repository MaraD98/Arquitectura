using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class AutomovilValidator : AbstractValidator<Automovil>
    {
        public AutomovilValidator()
        {
            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("La marca es obligatoria.")
                .MinimumLength(2).WithMessage("La marca debe tener al menos 2 caracteres.")
                .MaximumLength(50).WithMessage("La marca no puede superar los 50 caracteres.");

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("El modelo es obligatorio.")
                .MinimumLength(2).WithMessage("El modelo debe tener al menos 2 caracteres.")
                .MaximumLength(50).WithMessage("El modelo no puede superar los 50 caracteres.");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("El color es obligatorio.")
                .MinimumLength(3).WithMessage("El color debe tener al menos 3 caracteres.")
                .MaximumLength(30).WithMessage("El color no puede superar los 30 caracteres.");
        }
    }

}

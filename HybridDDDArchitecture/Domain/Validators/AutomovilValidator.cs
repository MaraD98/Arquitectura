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
            RuleFor(x => x.Marca).NotEmpty();
            RuleFor(x => x.Modelo).NotEmpty();
            RuleFor(x => x.Color).NotEmpty();
        }
    }
}

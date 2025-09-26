using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System;
using ValidationResult = FluentValidation.Results.ValidationResult;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities
{
    public interface IValidate
    {
        bool IsValid { get; }
        IList<ValidationFailure> GetErrors();
    }


    public class DomainEntity<TKey, TValidator> : IValidate
        where TValidator : IValidator, new()
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Decorador para que sea autoincremental
        public TKey Id { get; protected set; }
        public bool IsValid
        {
            get
            {
                Validate();
                return ValidationResult.IsValid;
            }
        }

        protected TValidator Validator { get; }
        private ValidationResult ValidationResult { get; set; }

        protected DomainEntity()
        {
            Validator = new TValidator();
        }

        protected void Validate()
        {
            var context = new ValidationContext<object>(this);
            ValidationResult = Validator.Validate(context);
        }

        public IList<ValidationFailure> GetErrors()
        {
            Validate();
            return ValidationResult.Errors;
        }
    }
}

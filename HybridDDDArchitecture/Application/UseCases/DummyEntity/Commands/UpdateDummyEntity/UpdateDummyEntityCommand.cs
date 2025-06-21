using Core.Application;
using MediatR;
using System.ComponentModel.DataAnnotations;
using static Domain.Enums.Enums;

namespace Application.UseCases.DummyEntity.Commands.UpdateDummyEntity
{
    public class UpdateDummyEntityCommand : IRequestCommand
    {
        [Required]
        public int DummyIdProperty { get; set; }
        [Required]
        public string DummyPropertyTwo { get; set; }
        public DummyValues DummyPropertyThree { get; set; }

        public UpdateDummyEntityCommand()
        {
        }
    }
}

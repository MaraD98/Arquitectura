using Core.Application;
using System.Collections.Generic;
using Application.DataTransferObjects;

namespace Application.UseCases.Automovil.Queries
{
    public class GetAllAutomovilesQuery : IRequestQuery<IEnumerable<AutomovilDto>>
    {
    }
}
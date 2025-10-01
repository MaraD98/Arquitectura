using Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Sql
{
    internal class AutomovilRepository : BaseRepository<Automovil>, IAutomovilRepository
    {
        public AutomovilRepository(StoreDbContext context) : base(context)
        {
        }
        public async Task<Automovil> FindOneByChasisAsync(string chasis)
        {
            // Utilizamos FirstOrDefaultAsync() para buscar el primer automóvil que coincida.
            // Asumo que 'Repository' es tu DbSet<Automovil> heredado de BaseRepository.
            return await Repository.FirstOrDefaultAsync(a => a.NumeroChasis == chasis);
        }
    }
}

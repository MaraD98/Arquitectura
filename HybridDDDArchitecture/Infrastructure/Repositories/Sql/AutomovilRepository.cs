using Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Entities;
using System;
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
    }
}

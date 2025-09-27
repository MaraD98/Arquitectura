using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DomainEvents
{
    internal class AutomovilEliminado : DomainEvent
    {
        public int AutomovilId { get; set; }

        public AutomovilEliminado(int id)
        {
            AutomovilId = id;
        }
    }
}

using Core.Application;

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

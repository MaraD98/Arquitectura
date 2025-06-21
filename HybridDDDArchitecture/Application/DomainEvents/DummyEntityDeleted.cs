using Core.Application;

namespace Application.DomainEvents
{
    internal sealed class DummyEntityDeleted : DomainEvent
    {
        public int DummyIdProperty { get; set; }

        public DummyEntityDeleted(int id)
        {
            DummyIdProperty = id;
        }
    }
}

namespace Core.Application
{
    public class DomainEvent : IRequestNotification
    {
        public DateTime EventDateUtc { get; private set; }

        public DomainEvent()
        {
            EventDateUtc = DateTime.UtcNow;
        }

        public DomainEvent(DateTime createDateUtc)
        {
            EventDateUtc = createDateUtc;
        }
    }
}

using Newtonsoft.Json;

namespace Core.Application
{
    public abstract class IntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreateDateUtc { get; private set; }

        [JsonProperty]
        public string EventType { get; private set; }

        [JsonProperty]
        public string Subject { get; private set; }

        [JsonProperty]
        public object Data { get; private set; }

        [JsonConstructor]
        protected IntegrationEvent()
        {
        }

        [JsonConstructor]
        protected IntegrationEvent(string eventType, string subject, object data)
        {
            Id = Guid.NewGuid();
            CreateDateUtc = DateTime.UtcNow.ToLocalTime();
            EventType = eventType;
            Subject = subject;
            Data = data;
        }

        [JsonConstructor]
        protected IntegrationEvent(Guid id, DateTime date, string eventType, string subject, object data)
        {
            Id = id;
            CreateDateUtc = date;
            EventType = eventType;
            Subject = subject;
            Data = data;
        }
    }
}

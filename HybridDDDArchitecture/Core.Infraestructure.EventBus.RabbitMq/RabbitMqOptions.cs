namespace Core.Infraestructure
{
    public class RabbitMqOptions
    {
        public string ConnectionString { get; set; }
        public bool? DispatchConsumersAsync { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
        public string UserName { get; set; }
        public string VirtualHost { get; set; }
    }
}

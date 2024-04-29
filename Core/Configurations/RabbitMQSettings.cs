namespace Core.Configurations
{
    public class RabbitMQSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

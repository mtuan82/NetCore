namespace Core.Providers.RabbitMQ
{
    public interface IRabbitMQProvider
    {
        void PublishQueue(string queueName, string message);
        List<string> ConsumeQueue(string queueName, ushort maxMessage);
        void DeleteQueue(string queueName);
    }
}

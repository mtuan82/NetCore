namespace NetCoreWebAPI.Services.RabbitMQ
{
    public interface IRabbitMQService
    {
        void pushQueue(string queueName, string message);

        List<string> popQueue(string queueName, ushort maxMessage);

        void deleteQueue(string queueName);
    }
}

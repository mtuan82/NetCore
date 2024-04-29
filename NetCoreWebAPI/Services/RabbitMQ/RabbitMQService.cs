using Core.Providers.RabbitMQ;

namespace NetCoreWebAPI.Services.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        IRabbitMQProvider _rabbitMQProvider;
        public RabbitMQService(IRabbitMQProvider RabbitMQProvider)
        {
            _rabbitMQProvider = RabbitMQProvider;
        }

        public void pushQueue(string queueName,string message)
        {
            _rabbitMQProvider.PublishQueue(queueName, message);
        }

        public List<string> popQueue(string queueName, ushort maxMessage)
        {
            return _rabbitMQProvider.ConsumeQueue(queueName, maxMessage);
        }

        public void deleteQueue(string queueName)
        {
            _rabbitMQProvider.DeleteQueue(queueName);
        }
    }
}

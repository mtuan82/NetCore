using Core.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Core.Providers.RabbitMQ
{
    public class RabbitMQProvider: IRabbitMQProvider
    {
        private readonly ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public RabbitMQProvider(RabbitMQSettings settings) {
            factory = new ConnectionFactory { 
                HostName = settings.Host,
                Port = settings.Port,
                Password = settings.Password,
                UserName = settings.UserName
            };
        }

        private void CreateConnectionChannel(string queueName)
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        private void DestroyConnectionChannel()
        {
            channel.Close();
            connection.Close();
        }

        public void PublishQueue(string queueName,string message)
        {
            CreateConnectionChannel(queueName);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queueName,
                                 basicProperties: null,                              
                                 body: body);
            DestroyConnectionChannel();
        }

        public List<string> ConsumeQueue(string queueName, ushort maxMessage)
        {
            int max = 0;
            List<string> message = new List<string>();
            CreateConnectionChannel(queueName);
            channel.BasicQos(0, maxMessage, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (max >= maxMessage)
                    return;
                var body = ea.Body.ToArray();
                message.Add(Encoding.UTF8.GetString(body));
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                max++;
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            while (message.Count < maxMessage)
            {
                Thread.Sleep(500);
            }
            DestroyConnectionChannel();
            return message;
        }

        public void DeleteQueue(string queueName)
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDelete(queue: queueName);
            DestroyConnectionChannel();
        }
    }
}

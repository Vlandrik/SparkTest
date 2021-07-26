using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SparkTest.MessageBroker.Constants;
using SparkTest.MessageBroker.Interfaces;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SparkTest.MessageBroker.Implementations
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ConnectionFactory _connectionFactory;

        public MessageBrokerService()
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };

        }

        public async Task PublishMessage(string message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: MessageBrokerConstants.QUEUE,
                    durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: MessageBrokerConstants.QUEUE,
                    basicProperties: null,
                    body: body);
            }
        }

        public async Task ReceiveMessage(Action<string> action)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: MessageBrokerConstants.QUEUE, durable: false,
                    exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Debug.WriteLine($"MESSAGE RECEIVED - {message}");

                    action.Invoke(message);
                };

                while (true)
                {
                    channel.BasicConsume(queue: MessageBrokerConstants.QUEUE, autoAck: true, consumer: consumer);

                    // Consume every second
                    await Task.Delay(1000);
                }
            }
        }
    }
}

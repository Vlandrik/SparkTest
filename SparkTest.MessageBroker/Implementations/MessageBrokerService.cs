using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SparkTest.MessageBroker.Constants;
using SparkTest.MessageBroker.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SparkTest.MessageBroker.Implementations
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private EventingBasicConsumer _consumer;
        private bool _consumerConfigured;

        public MessageBrokerService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: MessageBrokerConstants.QUEUE,
              durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        ~MessageBrokerService()
        {
            Dispose(false);
        }

        public async Task PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                routingKey: MessageBrokerConstants.QUEUE,
                basicProperties: null,
                body: body);
        }

        public void ConfigureMessageConsumer(Action<string> action)
        {
            var consumer = GetConsumer();

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                action.Invoke(message);
            };

            _consumerConfigured = true;
        }

        public void ConsumeMessage()
        {
            if (!_consumerConfigured)
                throw new Exception("Consumer hasn't been configured");

            _channel.BasicConsume(queue: MessageBrokerConstants.QUEUE, autoAck: true, consumer: GetConsumer());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_channel != null)
                    _channel.Close();

                if (_connection != null)
                    _connection.Close();
            }
        }

        private EventingBasicConsumer GetConsumer()
        {
            if (_consumer == null)
                _consumer = new EventingBasicConsumer(_channel);

            return _consumer;
        }
    }
}

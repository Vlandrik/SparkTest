using RabbitMQ.Client;
using SparkTest.MessageBroker.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace SparkTest.MessageBroker.Implementations
{
    public class MessageBrokerService : IMessageBrokerService
    {
        public async Task PublishMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}

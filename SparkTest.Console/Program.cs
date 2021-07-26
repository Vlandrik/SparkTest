using SparkTest.DAL.Domain.Entities;
using SparkTest.DAL.Implementations.DataContext;
using SparkTest.DAL.Implementations.Repository;
using SparkTest.MessageBroker.Implementations;
using SparkTest.Services.Implementations;
using System.Threading.Tasks;

namespace SparkTest.Console
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.ReadKey();

            var service = GetUserService();
            using var messageBrokerService = GetMessageBroker(service);

            while (true)
            {
                await ConsumeMessage(messageBrokerService);
            }
        }

        private static async Task ConsumeMessage(MessageBrokerService messageBrokerService)
        {
            messageBrokerService.ConsumeMessage();

            System.Console.WriteLine("MESSAGE CONSUMING IS GOING ON");

            await Task.Delay(1000);
        }

        private static UsersService GetUserService()
        {
            var dataContext = new DataContext("mongodb://localhost:27017", "SparkTest");

            var repository = new Repository<ApplicationUser>(dataContext);

            var service = new UsersService(repository);

            return service;
        }

        private static MessageBrokerService GetMessageBroker(UsersService service)
        {
            var messageBrokerService = new MessageBrokerService();

            messageBrokerService.ConfigureMessageConsumer((string message) =>
            {
                service.CreateUser(message).Wait();

                System.Console.WriteLine($"MESSAGE CONSUMED - {message}");
            });
            return messageBrokerService;
        }
    }
}

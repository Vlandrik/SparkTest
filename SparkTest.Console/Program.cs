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

            var dataContext = new DataContext("mongodb://localhost:27017", "SparkTest");

            var repository = new Repository<ApplicationUser>(dataContext);

            var service = new UsersService(repository);

            var messageBrokerService = new MessageBrokerService();

            await messageBrokerService.ReceiveMessage((string message) => service.CreateUser(message).Wait());
        }
    }
}

using System;
using System.Threading.Tasks;

namespace SparkTest.MessageBroker.Interfaces
{
    public interface IMessageBrokerService
    {
        Task PublishMessage(string message);

        Task ReceiveMessage(Action<string> action);
    }
}

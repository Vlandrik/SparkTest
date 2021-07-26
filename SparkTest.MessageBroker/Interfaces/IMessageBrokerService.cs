using System;
using System.Threading.Tasks;

namespace SparkTest.MessageBroker.Interfaces
{
    public interface IMessageBrokerService : IDisposable
    {
        Task PublishMessage(string message);

        void ConsumeMessage();

        void ConfigureMessageConsumer(Action<string> action);
    }
}

using SparkTest.MessageBroker.Interfaces;
using SparkTest.Services.Interfaces;
using System.Threading.Tasks;

namespace SparkTest.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageBrokerService _messageBrokerService;

        public MessageService(IMessageBrokerService messageBrokerService)
        {
            _messageBrokerService = messageBrokerService;
        }

        public async Task CreateUserMessage(string name)
        {
            await _messageBrokerService.PublishMessage(name);
        }
    }
}

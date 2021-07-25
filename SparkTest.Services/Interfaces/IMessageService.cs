using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SparkTest.Services.Interfaces
{
    public interface IMessageService
    {
        Task CreateUserMessage(string name);
    }
}

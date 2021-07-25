using SparkTest.DAL.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkTest.Services.Interfaces
{
    public interface IUsersService
    {
        Task<List<ApplicationUser>> GetUsers();

        Task CreateUser(string name);
    }
}

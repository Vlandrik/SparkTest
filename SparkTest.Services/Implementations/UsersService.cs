using SparkTest.DAL.Domain.Entities;
using SparkTest.DAL.Interfaces.Repository;
using SparkTest.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkTest.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<ApplicationUser> _userRepository;

        public UsersService(IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUser(string name)
        {
            await _userRepository.Insert(new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Name = name
            });
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            var result = await _userRepository.GetAll();

            return result;
        }
    }
}

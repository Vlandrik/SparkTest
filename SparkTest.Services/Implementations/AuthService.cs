using Microsoft.Extensions.Options;
using SparkTest.Services.Interfaces;
using SparkTest.Services.Models;
using SparkTest.Services.Settings;
using System;
using System.Threading.Tasks;

namespace SparkTest.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IJWTService _jwtService;
        private readonly UserInfo _userInfo;

        public AuthService(IJWTService jwtService, IOptions<UserInfo> userInfo)
        {
            _jwtService = jwtService;
            _userInfo = userInfo.Value;
        }

        public async Task<JWTTokenModel> GetTokenUserCredentials(CredentialsModel credentials)
        {
            // Data are open, in real world DB + hash would be used
            if (!_userInfo.Password.Equals(credentials.Password) || !_userInfo.Login.Equals(credentials.Email))
                throw new Exception("Invalid credentials");

            // Create temp artificial user
            var result = await _jwtService.CreateUserTokenAsync(new DAL.Domain.Entities.ApplicationUser
            {
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Name = "TestName"
            });

            return result;
        }
    }
}

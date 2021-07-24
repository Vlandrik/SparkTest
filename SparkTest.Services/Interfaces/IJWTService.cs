using SparkTest.DAL.Domain.Entities;
using SparkTest.Services.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SparkTest.Services.Interfaces
{
    public interface IJWTService
    {
        Task<ClaimsIdentity> GetIdentity(ApplicationUser user, bool isRefreshToken);

        JwtSecurityToken CreateToken(DateTime now, ClaimsIdentity identity, DateTime lifetime);

        Task<JWTTokenModel> CreateUserTokenAsync(ApplicationUser user);
    }
}

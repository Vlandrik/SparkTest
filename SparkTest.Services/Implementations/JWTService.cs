using Microsoft.IdentityModel.Tokens;
using SparkTest.DAL.Domain.Entities;
using SparkTest.Services.Helpers;
using SparkTest.Services.Interfaces;
using SparkTest.Services.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SparkTest.Services.Implementations
{
    public class JWTService : IJWTService
    {
        public JWTService()
        {
        }

        public async Task<ClaimsIdentity> GetIdentity(ApplicationUser user, bool isRefreshToken)
        {
            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        public JwtSecurityToken CreateToken(DateTime now, ClaimsIdentity identity, DateTime lifetime)
        {
            return new JwtSecurityToken(
                issuer: TokenOptionsHelper.ISSUER,
                audience: TokenOptionsHelper.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: lifetime,
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(TokenOptionsHelper.SIGNIN_KEY), SecurityAlgorithms.HmacSha256));
        }

        public async Task<JWTTokenModel> CreateUserTokenAsync(ApplicationUser user)
        {
            var dateNow = DateTime.UtcNow;

            var accessIdentity = await GetIdentity(user, false);

            if (accessIdentity == null)
            {
                throw new Exception("USER_NOT_FOUND");
            }

            var accessLifetime = dateNow.Add(TimeSpan.FromHours(1));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(CreateToken(dateNow, accessIdentity, accessLifetime));

            var response = new JWTTokenModel
            {
                AccessToken = accessToken,
                ExpireDate = accessLifetime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Type = "Bearer"
            };

            return response;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SparkTest.Services.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace SparkTest.API.Configurators
{
    public static class ConfigureJWTExtensions
    {
        public static void ConfigureJWT(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = TokenOptionsHelper.ISSUER,
                        ValidateAudience = true,
                        ValidateActor = false,
                        ValidAudience = TokenOptionsHelper.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = TokenOptionsHelper.GetSymmetricSecurityKey(TokenOptionsHelper.SIGNIN_KEY),
                        ValidateIssuerSigningKey = true,
                        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                        {
                            var token = securityToken as JwtSecurityToken;

                            if (!notBefore.HasValue || !expires.HasValue || DateTime.Compare(expires.Value, DateTime.UtcNow) <= 0)
                            {
                                return false;
                            }

                            if (token == null)
                            {
                                return false;
                            }

                            return true;
                        }
                    };
                });
        }

    }
}

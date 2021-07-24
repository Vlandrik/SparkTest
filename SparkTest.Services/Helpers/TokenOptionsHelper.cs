using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SparkTest.Services.Helpers
{
    public static class TokenOptionsHelper
    {
        public const int ACCESS_TOKEN_LIFETIME = 1;
        public const string ISSUER = "Issuer";
        public const string AUDIENCE = "Audience";
        public const string SIGNIN_KEY = "ServerKey";

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}

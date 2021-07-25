using SparkTest.Services.Models;
using System.Threading.Tasks;

namespace SparkTest.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JWTTokenModel> GetTokenUserCredentials(CredentialsModel credentials);
    }
}

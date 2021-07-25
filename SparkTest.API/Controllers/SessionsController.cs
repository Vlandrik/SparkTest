using Microsoft.AspNetCore.Mvc;
using SparkTest.API.Models;
using SparkTest.Services.Interfaces;
using SparkTest.Services.Models;
using System;
using System.Threading.Tasks;

namespace SparkTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public SessionsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(JWTTokenModel), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            try
            {
                var result = await _authService.GetTokenUserCredentials(new CredentialsModel
                {
                    Email = model.Email,
                    Password = model.Password
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}

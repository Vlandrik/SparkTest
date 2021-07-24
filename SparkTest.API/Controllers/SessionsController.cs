using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private IJWTService _jwtService;

        public SessionsController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(JWTTokenModel), 200)]
        public async Task<IActionResult> Login()
        {
            var result = await _jwtService.CreateUserTokenAsync(new DAL.Domain.Entities.ApplicationUser
            {
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Name = "TestName"
            });

            return Ok(result);
        }

        [HttpGet("test")]
        [ProducesResponseType(200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}

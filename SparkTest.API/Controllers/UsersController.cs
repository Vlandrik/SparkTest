using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkTest.DAL.Domain.Entities;
using SparkTest.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("test")]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            await _usersService.CreateUser("Test");
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationUser>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _usersService.GetUsers();

            return Ok(result);
        }
    }
}

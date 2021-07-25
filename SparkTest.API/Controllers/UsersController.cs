using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkTest.API.Models;
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
        private readonly IMessageService _messageSender;

        public UsersController(IUsersService usersService, IMessageService messageSender)
        {
            _usersService = usersService;
            _messageSender = messageSender;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(UserRequestModel model)
        {
            await _messageSender.CreateUserMessage(model.Name);

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

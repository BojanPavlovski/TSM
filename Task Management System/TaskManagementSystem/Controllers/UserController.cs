using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TaskManagementSystem.Dto.Users;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //a method that registers a user
        [AllowAnonymous]
        [HttpPost("registerUser")]
        public IActionResult RegisterUser([FromBody] AddUserDto model)
        {
            try
            {
                _userService.Register(model);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred!");
            }
        }
        //a method that allows a user to login, generating a token  later used for authorization
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult LogIn([FromBody] LogInDto model)
        {
            try
            {
                string token = _userService.LogIn(model);
                return Ok(token);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred!");
            }
        }
    }
}

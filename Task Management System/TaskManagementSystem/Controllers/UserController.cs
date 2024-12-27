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
        //a method that registers a user(adds user to database)
        [AllowAnonymous]
        [HttpPost("registerUser")]
        public IActionResult RegisterUser([FromBody] AddUserDto model)
        {
            try
            {
                //tries to make a call to the service layer(UserService)
                //if succesfull return Ok(200) status code
                _userService.Register(model);
                return Ok();
            }
            catch
            {
                //if an error occurs return status code 500(InternalServerError)
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
                //tries to make a call to the service layer(UserService)
                //if succesfull return status code ok(200)
                string token = _userService.LogIn(model);
                return Ok(token);
            }
            catch
            {
                //if an error occurs return status code 500(InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred!");
            }
        }
    }
}

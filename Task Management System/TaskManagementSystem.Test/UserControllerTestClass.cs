using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Dto.Users;
using TaskManagementSystem.Services.Implementations;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Test
{
    public class UserControllerTestClass
    {
        private readonly Mock<IUserService> _userService;
        private readonly UserController _userController;

        public UserControllerTestClass()
        {
            _userService = new Mock<IUserService>();
            _userController = new UserController(_userService.Object);
        }

        [Fact]
        public void RegisterUser_ShouldReturnOk_WhenUserIsRegisteredSuccesfully()
        {
            //Arange
            var userDto = new AddUserDto
            {
                Firstname = "John",
                Lastname = "Doe",
                UserName = "john123",
                Email = "john123@gmail.com",
                Password = "john123",
                ConfirmPassword = "john123"
            };

            _userService.Setup(service => service.Register(It.IsAny<AddUserDto>())).Verifiable();

            //Act
            var result = _userController.RegisterUser(userDto);

            //Assert
            var actionResult = Assert.IsType<OkResult>(result);
            _userService.Verify(service => service.Register(userDto), Times.Once);
        }

        [Fact]
        public void RegisterUser_ShouldReturnException_WhenUserRegisters()
        {
            //Arange
            var userDto = new AddUserDto
            {
                Firstname = "",
                Lastname = "",
                UserName = "john123",
                Email = "john123@gmail.com",
                Password = "john123",
                ConfirmPassword = "john123"
            };
            _userService.Setup(service => service.Register(It.IsAny<AddUserDto>())).Throws(new Exception("Test exception"));

            //Act
            var result = _userController.RegisterUser(userDto);

            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred!", objectResult.Value);
        }

        [Fact]
        public void RegisterUser_ShouldReturnException_WhenPasswordsDoNotMatch()
        {
            //Arange
            var userDto = new AddUserDto
            {
                Firstname = "John",
                Lastname = "Doe",
                UserName = "john123",
                Email = "john123@gmail.com",
                Password = "john123erer",
                ConfirmPassword = "john123f"
            };
            _userService.Setup(service => service.Register(It.IsAny<AddUserDto>())).Throws(new Exception("Test exception"));

            //Act
            var result = _userController.RegisterUser(userDto);

            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred!", objectResult.Value);
        }

        [Fact]
        public void RegisterUser_ShouldReturnException_WhenUsernameAndPasswordAreMissing()
        {
            //Arange
            var userDto = new AddUserDto
            {
                Firstname = "John",
                Lastname = "Doe",
                UserName = "",
                Email = "john123@gmail.com",
                Password = "",
                ConfirmPassword = ""
            };
            _userService.Setup(service => service.Register(It.IsAny<AddUserDto>())).Throws(new Exception("Test exception"));

            //Act
            var result = _userController.RegisterUser(userDto);

            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred!", objectResult.Value);
        }

        [Fact]
        public void LogIn_ShouldReturnOk_WhenUserIsLogedInSuccesfully()
        {
            //Arange
            var logInDto = new LogInDto
            {
                Username = "john123",
                Password = "john123"
            };

            var token = "some-jwt-token";
            _userService.Setup(service => service.LogIn(It.IsAny<LogInDto>())).Returns(token);

            // Act
            var result = _userController.LogIn(logInDto);

            // Assert
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)actionResult;
            Assert.Equal(token, okObjectResult.Value); //returns token
        }

        [Fact]
        public void LogIn_ShouldReturnInternalServerError_WhenUsernameIsMissing()
        {
            //Arrange
            var logInDto = new LogInDto
            {
                Username = "",
                Password = "john123"
            };
            _userService.Setup(service => service.LogIn(It.IsAny<LogInDto>())).Throws(new Exception("Username is invalid."));
            //Act
            var result = _userController.LogIn(logInDto);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode); 
            Assert.Equal("An error occurred!", objectResult.Value);
        }

        [Fact]
        public void LogIn_ShouldReturnInternalServerError_WhenPasswordIsMissing()
        {
            //Arrange
            var logInDto = new LogInDto
            {
                Username = "john123",
                Password = ""
            };
            _userService.Setup(service => service.LogIn(It.IsAny<LogInDto>())).Throws(new Exception("Password is invalid."));
            //Act
            var result = _userController.LogIn(logInDto);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode); 
            Assert.Equal("An error occurred!", objectResult.Value);
        }

        [Fact]
        public void LogIn_ShouldReturnInternalServerError_WhenPasswordAndUsernameAreMissing()
        {
            //Arrange
            var logInDto = new LogInDto
            {
                Username = "",
                Password = ""
            };
            _userService.Setup(service => service.LogIn(It.IsAny<LogInDto>())).Throws(new Exception("Test exception."));
            //Act
            var result = _userController.LogIn(logInDto);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred!", objectResult.Value);
        }


    }
}

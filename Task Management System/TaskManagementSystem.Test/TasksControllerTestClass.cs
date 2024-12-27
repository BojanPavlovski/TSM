using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Dto.Tasks;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Test
{
    public class TasksControllerTestClass
    {
        private readonly Mock<ITaskModelService> _taskModelService;
        private readonly TasksController _tasksController;

        public TasksControllerTestClass()
        {
            _taskModelService = new Mock<ITaskModelService>();
            _tasksController = new TasksController(_taskModelService.Object);
        }

        [Fact]
        public void GetTasks_ShouldReturnOk_WithValidResults()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto { 
                Id = 1, Name = "Test Task", Description = "Test Task Description", IsCompleted = true}, 
                new TaskModelDto{Id = 2, Name = "Second Test Task", Description = "Second Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = false}
            };

            _taskModelService.Setup(service => service.GetAllTasks(pageNumber, pageSize)).Returns(mockTasks);

            // Act
            var result =  _tasksController.Get(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskModelDto>>(okResult.Value);
            Assert.Equal(mockTasks.Count, returnValue.Count);
            _taskModelService.Verify(service => service.GetAllTasks(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public void GetTasks_ShouldReturnOk_WhenTheListIsEmpty()
        {
            //Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var mockTasks = new List<TaskModelDto>();

            _taskModelService.Setup(service => service.GetAllTasks(pageNumber, pageSize)).Returns(mockTasks);
            //Act
            var result = _tasksController.Get(pageNumber, pageSize);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskModelDto>>(okResult.Value);
            Assert.Equal(mockTasks.Count, returnValue.Count());
        }

        [Fact]
        public void AddTask_ShouldReturnOk_WhenValidDataIsSend()
        {
            //Arrange
            var taskModelDto = new TaskModelDto
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description",
                IsCompleted = true,
            };

            _taskModelService.Setup(service => service.AddTask(taskModelDto)).Verifiable();
            //Act
            var result = _tasksController.AddTask(taskModelDto);
            //Assert
            var okResult = Assert.IsType<StatusCodeResult>(result);
            _taskModelService.Verify(service => service.AddTask(taskModelDto), Times.Once());
        }

        [Fact]
        public void AddTask_ShouldReturnException_WhenInvalidDataIsProvided()
        {
            var taskModelDto = new TaskModelDto
            {
                Id = 1,
                Name = "",
                Description = "Test Description",
                IsCompleted = false,
            };

            _taskModelService.Setup(service => service.AddTask(taskModelDto)).Throws(new Exception("Test Exception"));
            //Act
            var result = _tasksController.AddTask(taskModelDto);
            //Assert
            var actionResult = Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)actionResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal(actionResult.ToString(), objectResult.ToString());
        }

        [Fact]
        public void DeleteTask_ShouldReturn_Status204NoContent()
        {
            //Arrange
            var taskId = 1;
            _taskModelService.Setup(service => service.DeleteTask(taskId)).Verifiable();
            //Act
            var result = _tasksController.Delete(taskId);
            //Assert
            var noContentResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            _taskModelService.Verify(s => s.DeleteTask(taskId), Times.Once);
        }

        [Fact]
        public void DeleteTask_ShouldReturnException_WhenTaskIdIsInvalid()
        {
            //Arrange
            var taskId = 22222;
            _taskModelService.Setup(service => service.DeleteTask(taskId)).Throws(new Exception("Task not found"));

            //Act
            var result = _tasksController.Delete(taskId);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(actionResult.StatusCode, objectResult.StatusCode);
        }

        [Fact]
        public void DeleteTask_ShouldReturnException_WhenTaskIdIsNull()
        {
            //Arrange
            var taskId = -1;
            _taskModelService.Setup(service => service.DeleteTask(taskId)).Throws<ArgumentNullException>();

            //Act
            var result = _tasksController.Delete(taskId);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(actionResult.ToString(), objectResult.ToString());
        }

        [Fact]
        public void UpdateTask_ShouldReturn_StatusCode204_WhenCalledSuccesfully()
        {
            //Arrange
            var taskModelDto = new TaskModelDto
            {
                Id = 1,
                Name = "Task Test Name",
                Description = "Task Test Description",
                IsCompleted = true
            };
            _taskModelService.Setup(service => service.UpdateTask(taskModelDto)).Verifiable();
            //Act
            var result = _tasksController.UpdateTask(taskModelDto);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, actionResult.StatusCode);
            _taskModelService.Verify(service => service.UpdateTask(taskModelDto), Times.Once);
        }

        [Fact]
        public void UpdateTask_ShouldReturnException_WhenDataIsInvalid()
        {
            //Arrange
            var taskModelDto = new TaskModelDto
            {
                Id = 1,
                Name = "",
                Description = "",
                IsCompleted= false
            };
            _taskModelService.Setup(service => service.UpdateTask(taskModelDto)).Throws<Exception>();
            //Act
            var result = _tasksController.UpdateTask(taskModelDto);
            //Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(actionResult.StatusCode, objectResult.StatusCode);
        }

        [Fact]
        public void GetTaskByName_ReturnsOkStatus_WhenSucesfull()
        {
            //Arrange
            string taskName = "Test Task";
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto {
                Id = 1, Name = "Test Task", Description = "Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 2, Name = "Test Task", Description = "Second Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = false}
            };
            _taskModelService.Setup(service => service.GetTask(taskName, pageNumber, pageSize)).Returns(mockTasks);
            //Act
            var result = _tasksController.GetTaskByTaskName(taskName, pageNumber, pageSize);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TaskModelDto>>(okResult.Value);
            Assert.Equal(mockTasks.Count, returnValue.Count);
            _taskModelService.Verify(service => service.GetTask(taskName,pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public void GetTask_ReturnsException_WhenUnsuccesfull()
        {
            //Arrange
            string taskName = "name";
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto {
                Id = 1, Name = "Test Task", Description = "Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 2, Name = "Test Task", Description = "Second Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = false}
            };
            _taskModelService.Setup(service => service.GetTask(taskName, pageNumber, pageSize)).Throws(new Exception());
            //Act
            var result = _tasksController.GetTaskByTaskName(taskName, pageNumber,pageSize);
            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = (BadRequestObjectResult)actionResult;
            Assert.Equal(actionResult.StatusCode, returnValue.StatusCode);
        }

        [Fact]
        public void GetTask_ReturnsException_WhenTaskNameIsEmpty()
        {
            //Arrange
            string taskName = "";
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto {
                Id = 1, Name = "Test Task", Description = "Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 2, Name = "Test Task", Description = "Second Test Task Description", IsCompleted = true},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = false}
            };
            _taskModelService.Setup(service => service.GetTask(taskName, pageNumber, pageSize)).Throws(new Exception());
            //Act
            var result = _tasksController.GetTaskByTaskName(taskName, pageNumber, pageSize);
            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = (BadRequestObjectResult)actionResult;
            Assert.Equal(actionResult.StatusCode, returnValue.StatusCode );
        }

        [Fact]
        public void GetTaskBasedOndDescription_ShouldReturnOkStatus()
        {
            string taskDescription = "Test Task Description";
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto {
                Id = 1, Name = "Test Task", Description = taskDescription, IsCompleted = true},
                new TaskModelDto{Id = 2, Name = "Test Task", Description = taskDescription, IsCompleted = true},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = false}
            };
            _taskModelService.Setup(service => service.GetTasksBasedOnDescription(taskDescription, pageNumber, pageSize)).Returns(mockTasks);
            //Act
            var result = _tasksController.GetTaskBasedOndDescription(taskDescription, pageNumber, pageSize);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = (OkObjectResult)okResult;
            Assert.Equal(okResult.StatusCode, returnValue.StatusCode);
        }

        [Fact]
        public void GetTaskBasedOnDescription_ShouldReturnEmptyList_WhenTaskDescriptionDoesNotExist()
        {
            //Arrange
            string taskDescription = "aaa";
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto {
                Id = 1, Name = "Test Task", Description = "desc1", IsCompleted = true},
                new TaskModelDto{Id = 2, Name = "Test Task", Description = "desc2", IsCompleted = true},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = false}
            };
            _taskModelService.Setup(service => service.GetTasksBasedOnDescription(taskDescription, pageNumber, pageSize)).Returns(mockTasks);
            //Act
            var result = _tasksController.GetTaskBasedOndDescription(taskDescription, pageNumber, pageSize);
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = (ObjectResult)okResult;
            Assert.Equal(okResult.StatusCode, returnValue.StatusCode);
        }

        [Fact]
        public void GetTaskBasedOnDescription_ShouldReturnEmptyList_WhenTaskDescriptionIsEmpty()
        {
            //Arrange
            string taskDescription = "";
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto>();
            _taskModelService.Setup(service => service.GetTasksBasedOnDescription(taskDescription, pageNumber, pageSize)).Returns(mockTasks);
            //Act
            var result = _tasksController.GetTaskBasedOndDescription(taskDescription, pageNumber, pageSize);
            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = (ObjectResult)actionResult;
            Assert.Equal(actionResult.StatusCode, returnValue.StatusCode);
        }

        [Fact]
        public void GetTaskBasedOnStatus_ShouldReturnOkStatus_WhenSuccesfull()
        {
            //Arrange
            bool status = true;
            int pageNumber = 1;
            int pageSize = 10;
            var mockTasks = new List<TaskModelDto> { new TaskModelDto {
                Id = 1, Name = "Test Task", Description = "some desc", IsCompleted = status},
                new TaskModelDto{Id = 2, Name = "Test Task", Description = "some desc", IsCompleted = status},
                new TaskModelDto{Id = 3, Name = "Third Test Task", Description = "Third Test Task Description", IsCompleted = status}
            };
            _taskModelService.Setup(service => service.GetTasksBasedOnStatus(status, pageNumber, pageSize)).Returns(mockTasks);
            //Act
            var result = _tasksController.GetTasksBasedOnStatus(status, pageNumber, pageSize);
            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = (OkObjectResult)actionResult;
            Assert.Equal(actionResult.StatusCode, returnValue.StatusCode);
        }


    }
}

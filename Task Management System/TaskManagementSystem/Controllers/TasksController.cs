using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Dto.Tasks;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskModelService _taskModelService;
        public TasksController(ITaskModelService taskModelService)
        {
            _taskModelService = taskModelService;
        }
        //A method that returns a list of all the Tasks stored in the database
        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber, int pageSize)
        {
            try
            {
                //making a call to the service layer(TaskModelService), who makes a call to the repository to retrieve the list
                //returns status code 200 if succesfull
                //pagination
                var data = _taskModelService.GetAllTasks(pageNumber, pageSize);
                
                return Ok(data);

            }
            catch (Exception ex)
            {
                //catching the error if it occures, display the error message
                return BadRequest(ex.Message);
            }
        }

        //a method that adds a new Task to the database
        [HttpPost("addTask")]
        public IActionResult AddTask([FromBody] TaskModelDto task)
        {
            try
            {
                //making a call to the service layer(TaskModelService)
                _taskModelService.AddTask(task);
                //if succesfull return status code 201
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                //catching the error if it occures, return status code 500
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //a method that deletes a Task resource
        [HttpDelete("deleteTask")]
        public IActionResult Delete(int id)
        {
            try
            {
                //tries to make a call to the service layer(TaskModelService)
                _taskModelService.DeleteTask(id);
                //if deletion is succesfull, return status code 204 and display message
                return StatusCode(StatusCodes.Status204NoContent, "Deleted resource.");
            }
            catch (Exception ex)
            {
                //if an error occurs on the server side return status code 500 and display the error of the message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //a method that Updated a Task resource
        [HttpPut("updateTask")]
        public IActionResult UpdateTask([FromBody] TaskModelDto task)
        {
            try
            {
                //tries to make a call to the service layer to handle updating the resource
                _taskModelService.UpdateTask(task);
                //if updating is succesfull, return sttaus code 204 and the appropriate message
                return StatusCode(StatusCodes.Status204NoContent, "Task is updated.");
            }
            catch(Exception ex)
            {
                //if an error occurs while updating the Task, return status code 500
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
        }

        //a method that gets a specific Task, based on Task name
        [HttpGet("getTaskByTitle")]
        public ActionResult GetTask(string taskName, int pageNumber, int pageSize)
        {
            try
            {
                //tries to make a call to the service layer(TaskModelService)
                return Ok(_taskModelService.GetTask(taskName, pageSize, pageNumber));
            }
            catch (Exception ex)
            {
                //if the task does not exist, return a BadRequest
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getTasksBasedOnDescription")]
        public ActionResult GetTaskBasedOndDescription(string taskName, int pageNumber, int pageSize)
        {
            try
            {
                return Ok(_taskModelService.GetTasksBasedOnDescription(taskName, pageSize, pageNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //a method that returns a list of tasks, based on their completion status
        [HttpGet("getTasksBasedOnStatus")]
        public ActionResult GetTasksBasedOnStatus(bool isCompleteed, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                //making a call to the service layer(TaskModelService)
                //if succesfull return status code 200
                //pagination
                var data = _taskModelService.GetTasksBasedOnStatus(isCompleteed, pageNumber, pageSize);
                return Ok(data);
            }
            catch (Exception ex)
            {
                //if an error occurs while retriving resourse return BadRequest
                return BadRequest(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.Domain.Domain;

namespace TaskManagementSystem.DataAccess.Implementations
{
    //interface implementations
    public class TaskModelRepository : ITaskRepository
    {
        //dependency
        private TaskManagementSystemDbContext _dbContext;
        public TaskModelRepository(TaskManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //a method that makes a call to the database to add a TaskModel
        public void Add(TaskModel entity)
        {
            //establishing and closing a connection to database to add a TaskModel resource
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
        }

        //a method that makes a call to the database to delete a resource
        public void Delete(TaskModel entity)
        {
            //establishing and closing a connection to database, when deleting a resource
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }
        //a method that calls the database and retrieves all TaskModel resources
        public List<TaskModel> GetAll(int pageNumber, int pageSize)
        {
            //calling the database
            return _dbContext.Tasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        //a method that makes a call to the database to retrieve a TaskModel resource based on task title(name)
        public List<TaskModel> GetTaskByTitle(string taskTitle, int pageNumber, int pageSize)
        {
            //calling the database 
            //pagination
            return _dbContext.Tasks.Where(x => x.Name.Contains(taskTitle))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        //a method that makes a call to the database and returns a list of Tasks, based on description
        public List<TaskModel> GetTaskByDescription(string description, int pageNumber, int pageSize)
        {
            return _dbContext.Tasks.Where(x =>  x.Description.Contains(description))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        //a method that makes a call to the databse and returns a Task, based on TaskModel id
        public TaskModel GetTaskById(int taskId)
        {
            return _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);
        }

        //a method that calls the database to retrieve a list of TaskModel resources based on their status
        public List<TaskModel> GetTaskByStatus(bool isCompleted, int pageNumber, int pageSize)
        {
            //calling the database and filtering resources based on their sttaus
            return _dbContext.Tasks.Where(x => x.IsCompleted == isCompleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        //a method that calls the database and returns the count(total number) of tasks
        public int GetTaskCount()
        {
            return _dbContext.Tasks.Count();
        }

        //a method that makes a call to the database ti update the specific TaskModel resource
        public void Update(TaskModel entity)
        {
            //establishing and closing a connection with database when updating resource
            _dbContext.Tasks.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}

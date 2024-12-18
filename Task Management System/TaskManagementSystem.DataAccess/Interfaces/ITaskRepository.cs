using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Domain;

namespace TaskManagementSystem.DataAccess.Interfaces
{
    //a seperate interface for specific TaskModel operations, seperate from CRUD operations
    public interface ITaskRepository : ITaskModelRepository<TaskModel>
    {
        List<TaskModel> GetTaskByTitle(string taskTitle, int pageNumber, int pageSize);
        List<TaskModel> GetTaskByStatus(bool isCompleted, int pageNumber, int pageSize);
        List<TaskModel> GetTaskByDescription(string description, int pageNumber, int pageSize);
        int GetTaskCount();
        TaskModel GetTaskById(int taskId);
    }
}

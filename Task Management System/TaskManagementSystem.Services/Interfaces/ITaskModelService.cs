using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Dto.Tasks;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskModelService
    {
        List<TaskModelDto> GetAllTasks(int pageNumber, int pageSize);
        void AddTask(TaskModelDto task);
        void DeleteTask(int id);
        void UpdateTask(TaskModelDto task);
        List<TaskModelDto> GetTask(string taskName, int pageNumber, int pageSize);
        List<TaskModelDto> GetTasksBasedOnDescription(string description, int pageNumber, int pageSize);
        List<TaskModelDto> GetTasksBasedOnStatus(bool isCompleted, int pageNumber, int pageSize);
        int GetTaskCount();
    }
}

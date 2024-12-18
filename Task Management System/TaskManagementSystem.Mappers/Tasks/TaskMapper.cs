using TaskManagementSystem.Domain.Domain;
using TaskManagementSystem.Dto.Tasks;

namespace TaskManagementSystem.Mappers.Tasks
{
    public static class TaskMapper
    {  
        //mapping from a domain (TaskModel) object to a Data Transfer Object(TaskModelDto)
        public static TaskModelDto toTaskDto(this TaskModel taskModel)
        {
            return new TaskModelDto
            {
                Id = taskModel.Id,
                Name = taskModel.Name,
                Description = taskModel.Description,
                IsCompleted = taskModel.IsCompleted
            };
        }

        //mapping from Data Transfer Object(TaskModelDto) to a domain object(TaskModel)
        public static TaskModel toTaskModel(this TaskModelDto task)
        {
            return new TaskModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }
    }
}

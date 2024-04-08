using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITaskService
    {
        // Methods for managing Tasks
        Task<TaskDto> AddTaskToTodoListAsync(int todoListId, TaskDto taskDto);
        Task<TaskDto> UpdateTaskAsync(int taskId, TaskDto taskDto);
        Task<TaskDto> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskDto>> GetTasksByAssignedUserIdAsync(string userId);
        Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(string userId);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<bool> AddTagToTaskAsync(int taskId, int tagId);

    }
}

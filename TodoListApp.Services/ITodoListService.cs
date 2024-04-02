

using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITodoListService
    {
        Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto);
        Task<TodoListDto> GetTodoListByIdAsync(int id);
        Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync();
        Task<TodoListDto> UpdateTodoListAsync(int id, TodoListDto todoListDto);
        Task<bool> DeleteTodoListAsync(int id);
        Task<TaskDto> AddTaskToTodoListAsync(int todoListId, TaskDto taskDto);
        Task<TaskDto> UpdateTaskAsync(int taskId, TaskDto taskDto);
        Task<TaskDto> GetTaskByIdAsync(int taskId);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<TagDto> AddTagToTaskAsync(int taskId, TagDto tagDto);
        Task<IEnumerable<TagDto>> GetTagsByTaskIdAsync(int taskId);
        Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId);

        // Methods for managing Comments
        Task<CommentDto> AddCommentToTaskAsync(int taskId, CommentDto commentDto);
        Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(int taskId);
        Task<CommentDto> UpdateCommentAsync(int commentId, CommentDto commentDto);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}

using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITodoListService
    {

        // Methods for managing Lists
        Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto);
        Task<TodoListDto> GetTodoListByIdAsync(int id);
        Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync(string userId);
        Task<TodoListDto> UpdateTodoListAsync(int id, TodoListDto todoListDto);
        Task<bool> DeleteTodoListAsync(int id);
    }
}
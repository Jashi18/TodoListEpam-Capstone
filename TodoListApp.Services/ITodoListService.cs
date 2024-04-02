using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITodoListService
    {
        Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync();
        Task<TodoListDto> GetTodoListByIdAsync(int id);
        Task<TodoListDto> CreateTodoListAsync(TodoListDto todoList);
        Task UpdateTodoListAsync(TodoListDto todoList);
        Task DeleteTodoListAsync(int id);
    }
}

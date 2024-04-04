using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TaskWebApiService
    {
        private readonly HttpClient _httpClient;

        public TaskWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByTodoListIdAsync(int todoListId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskDto>>($"https://localhost:7000/api/TodoList/{todoListId}/tasks");
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId)
        {
            return await _httpClient.GetFromJsonAsync<TaskDto>($"https://localhost:7000/api/Todolist/Tasks/{taskId}");
        }

        public async Task<TaskDto> AddTaskToTodoListAsync(int todoListId, TaskDto taskDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7000/api/TodoList/{todoListId}/tasks", taskDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskDto>();
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7000/api/Todolist/Tasks/{taskId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AssignTaskToUserAsync(int taskId, string userId)
        {
            var response = await _httpClient.PutAsync($"https://localhost:7000/api/TodoList/Tasks/{taskId}/Assign/{userId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTaskAsync(TaskDto taskDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7000/api/Todolist/Tasks/{taskDto.Id}", taskDto);
            return response.IsSuccessStatusCode;
        }
    }
}

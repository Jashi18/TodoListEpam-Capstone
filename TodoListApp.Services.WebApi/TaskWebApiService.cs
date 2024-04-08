using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TaskWebApiService
    {
        private readonly HttpClient _httpClient;
        public string _baseUri = "https://localhost:7000/api/Task/tasks";
        public TaskWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByTodoListIdAsync(int todoListId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskDto>>($"{_baseUri}/{todoListId}/tasks");
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId)
        {

            return await _httpClient.GetFromJsonAsync<TaskDto>($"https://localhost:7000/api/Task/tasks/{taskId}");
        }

        public async Task<TaskDto> AddTaskToTodoListAsync(int todoListId, TaskDto taskDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7000/api/Task/{todoListId}/tasks", taskDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskDto>();
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUri}/{taskId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTaskAsync(int taskId, TaskDto taskDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUri}/{taskId}", taskDto);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAssignedToUserAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskDto>>($"https://localhost:7000/api/Task/assigned/{userId}");
        }

        public async Task<bool> AddTagToTaskAsync(int taskId, int tagId)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7000/api/tasks/{taskId}/tags", new { tagId = tagId });

            return response.IsSuccessStatusCode;
        }
    }
}

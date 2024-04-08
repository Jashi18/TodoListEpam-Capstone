using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TodoListWebApiService
    {
        private readonly HttpClient _httpClient;
        public TodoListWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"TodoList/?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<TodoListDto>>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed with status code {response.StatusCode}: {errorContent}");
            }
        }

        public async Task<bool> CreateTodoListAsync(TodoListDto todoList)
        {
            var response = await _httpClient.PostAsJsonAsync($"TodoList/", todoList);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTodoListAsync(int id, TodoListDto todoList)
        {
            var response = await _httpClient.PutAsJsonAsync($"TodoList/{id}", todoList);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTodoListAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"TodoList/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<TodoListDto> GetTodoListByIdAsync(int id)
        {   
            var response = await _httpClient.GetAsync($"TodoList/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TodoListDto>();
            }
            else
            {
                return null;
            }
        }
    }
}

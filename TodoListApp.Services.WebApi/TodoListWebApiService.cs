using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TodoListWebApiService
    {
        private readonly HttpClient _httpClient;
        public string _baseUri = "https://localhost:7000/api/TodoList";
        public TodoListWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_baseUri}?userId={userId}");

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
            var response = await _httpClient.PostAsJsonAsync(_baseUri, todoList);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTodoListAsync(int id, TodoListDto todoList)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUri}/{id}", todoList);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTodoListAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUri}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<TodoListDto> GetTodoListByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7000/api/TodoList/{id}");

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

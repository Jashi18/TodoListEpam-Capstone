using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class CommentWebApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "https://localhost:7000/api/Comment/comments";

        public CommentWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CommentDto> GetCommentByIdAsync(int commentId)
        {
            return await _httpClient.GetFromJsonAsync<CommentDto>($"{_baseUri}/{commentId}");
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(int taskId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CommentDto>>($"{_baseUri}/task/{taskId}");
        }

        public async Task<CommentDto> AddCommentToTaskAsync(int taskId, CommentDto commentDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7000/api/Comment/tasks/{taskId}/comments", commentDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CommentDto>();
        }

        public async Task<bool> UpdateCommentAsync(CommentDto commentDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUri}/{commentDto.Id}", commentDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUri}/{commentId}");
            return response.IsSuccessStatusCode;
        }
    }
}

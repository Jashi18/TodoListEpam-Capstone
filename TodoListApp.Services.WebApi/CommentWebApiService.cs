using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class CommentWebApiService
    {
        private readonly HttpClient _httpClient;

        public CommentWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CommentDto> GetCommentByIdAsync(int commentId)
        {
            return await _httpClient.GetFromJsonAsync<CommentDto>($"Comment/comments/{commentId}");
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(int taskId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CommentDto>>($"Comment/tasks/{taskId}/comments");
        }

        public async Task<CommentDto> AddCommentToTaskAsync(int taskId, CommentDto commentDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"Comment/tasks/{taskId}/comments", commentDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CommentDto>();
        }

        public async Task<bool> UpdateCommentAsync(CommentDto commentDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Comment/comments/{commentDto.Id}", commentDto);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var response = await _httpClient.DeleteAsync($"Comment/comments/{commentId}");
            return response.IsSuccessStatusCode;
        }
    }
}

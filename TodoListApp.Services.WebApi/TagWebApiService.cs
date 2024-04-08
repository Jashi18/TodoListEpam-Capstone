using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

public class TagWebApiService
{
    private readonly HttpClient _httpClient;

    public TagWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var response = await _httpClient.GetAsync($"Tag/tags");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<TagDto>>();
    }

    public async Task<TagDto> CreateTagAsync(TagDto tagDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"Tag/tags", tagDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagDto>();
    }

    public async Task<bool> DeleteTagAsync(int tagId)
    {
        var response = await _httpClient.DeleteAsync($"Tag/tags/{tagId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var url = $"Tag/tasks/{taskId}/tags/{tagId}";

        var response = await _httpClient.DeleteAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error removing tag from task: {errorContent}");
        }
        return response.IsSuccessStatusCode;
    }
}

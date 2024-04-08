using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

public class TagWebApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _tagsApiUrl;

    public TagWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _tagsApiUrl = "https://localhost:7000/tags";
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var response = await _httpClient.GetAsync(_tagsApiUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<TagDto>>();
    }

    public async Task<TagDto> CreateTagAsync(TagDto tagDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_tagsApiUrl, tagDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TagDto>();
    }

    public async Task<bool> DeleteTagAsync(int tagId)
    {
        var response = await _httpClient.DeleteAsync($"{_tagsApiUrl}/{tagId}");
        return response.IsSuccessStatusCode;
    }


}

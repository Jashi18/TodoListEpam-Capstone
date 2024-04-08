using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ITagService
    {
        // Methods for managing Tags
        Task<TagDto> CreateTagAsync(TagDto tagDto);
        Task<IEnumerable<TagDto>> GetTagsByTaskIdAsync(int taskId);
        Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId);
        Task<TagDto> GetTagByIdAsync(int id);
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<bool> DeleteTagAsync(int tagId);
    }
}

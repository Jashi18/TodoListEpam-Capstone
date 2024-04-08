using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface ICommentService
    {
        // Methods for managing Comments
        Task<CommentDto> AddCommentToTaskAsync(int taskId, CommentDto commentDto);
        Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(int taskId);
        Task<CommentDto> GetCommentByIdAsync(int commentId);
        Task<CommentDto> UpdateCommentAsync(int commentId, CommentDto commentDto);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}

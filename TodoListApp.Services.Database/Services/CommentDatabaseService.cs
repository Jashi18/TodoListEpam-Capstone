using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database.Services
{
    public class CommentDatabaseService : ICommentService
    {
        private readonly TodoListDbContext _context;

        public CommentDatabaseService(TodoListDbContext context)
        {
            _context = context;
        }

        public async Task<CommentDto> AddCommentToTaskAsync(int taskId, CommentDto commentDto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;
            var comment = new CommentEntity { 
                Text = commentDto.Text, 
                TaskId = taskId, 
                UserName = commentDto.UserName
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return new CommentDto 
            { 
                Id = comment.Id, 
                Text = comment.Text,
                CreatedAt = comment.CreatedAt, 
                UserName = comment.UserName 
            };
        }

        public async Task<CommentDto> GetCommentByIdAsync(int commentId)
        {
            var comment = await _context.Comments
                .Where(c => c.Id == commentId)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt,
                    TaskId = c.TaskId,
                    UserName = c.UserName
                })
                .FirstOrDefaultAsync();

            return comment;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(int taskId)
        {
            var comments = await _context.Comments
                .Where(c => c.TaskId == taskId)
                .Select(c => new CommentDto { 
                    Id = c.Id, Text = c.Text, 
                    TaskId=c.TaskId, 
                    CreatedAt = c.CreatedAt, 
                    UserName = c.UserName })
                .ToListAsync();

            return comments;
        }

        public async Task<CommentDto> UpdateCommentAsync(int commentId, CommentDto commentDto)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return null;

            comment.Text = commentDto.Text;
            await _context.SaveChangesAsync();

            return new CommentDto { 
                Id = comment.Id, 
                Text = comment.Text, 
                CreatedAt = comment.CreatedAt 
            };
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentListService)
        {
            _commentService = commentListService;
        }

        [HttpPost("tasks/{taskId}/comments")]
        public async Task<IActionResult> AddCommentToTask(int taskId,  [FromBody] CommentDto commentDto)
        {
            var comment = new CommentDto
            {
                Id = commentDto.Id,
                Text = commentDto.Text,
                TaskId = taskId,
                UserName = commentDto.UserName,
                CreatedAt = DateTime.Now,
            };
            await _commentService.AddCommentToTaskAsync(taskId, comment);
            if (comment == null) return NotFound($"Task with ID {taskId} not found.");
            return Ok(comment);
        }


        [HttpGet("tasks/{taskId}/comments")]
        public async Task<IActionResult> GetCommentsByTaskId(int taskId)
        {
            var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        [HttpPut("comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentDto commentDto)
        {
            var updatedComment = await _commentService.UpdateCommentAsync(commentId, commentDto);
            if (updatedComment == null) return NotFound($"Comment with ID {commentId} not found.");
            return Ok(updatedComment);
        }
        [HttpGet("comments/{commentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound($"Comment with ID {commentId} not found.");
            }
            return Ok(comment);
        }

        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var result = await _commentService.DeleteCommentAsync(commentId);
            if (!result) return NotFound($"Comment with ID {commentId} not found.");
            return NoContent();
        }
    }
}

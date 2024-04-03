using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoListDto todoListDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdTodoList = await _todoListService.CreateTodoListAsync(todoListDto, userId);
            return CreatedAtAction(nameof(GetTodoList), new { id = createdTodoList.Id }, createdTodoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoList(int id)
        {
            var todoList = await _todoListService.GetTodoListByIdAsync(id);
            if (todoList == null) return NotFound();
            return Ok(todoList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodoLists(string userId)
        {
            var todoLists = await _todoListService.GetAllTodoListsAsync(userId);
            return Ok(todoLists);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoList(int id, [FromBody] TodoListDto todoListDto)
        {
            var updatedTodoList = await _todoListService.UpdateTodoListAsync(id, todoListDto);
            if (updatedTodoList == null) return NotFound();
            return Ok(updatedTodoList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            var result = await _todoListService.DeleteTodoListAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{todoListId}/tasks")]
        public async Task<IActionResult> AddTaskToTodoList(int todoListId, [FromBody] TaskDto taskDto)
        {
            var addedTask = await _todoListService.AddTaskToTodoListAsync(todoListId, taskDto);
            if (addedTask == null) return NotFound();
            return Ok(addedTask);
        }

        [HttpPut("tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDto taskDto)
        {
            var updatedTask = await _todoListService.UpdateTaskAsync(taskId, taskDto);
            if (updatedTask == null) return NotFound();
            return Ok(updatedTask);
        }

        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var result = await _todoListService.DeleteTaskAsync(taskId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("tasks/{taskId}/tags")]
        public async Task<IActionResult> AddTagToTask(int taskId, [FromBody] TagDto tagDto)
        {
            var tag = await _todoListService.AddTagToTaskAsync(taskId, tagDto);
            if (tag == null) return NotFound($"Task with ID {taskId} not found.");
            return Ok(tag);
        }

        [HttpGet("tasks/{taskId}/tags")]
        public async Task<IActionResult> GetTagsByTaskId(int taskId)
        {
            var tags = await _todoListService.GetTagsByTaskIdAsync(taskId);
            return Ok(tags);
        }

        [HttpDelete("tasks/{taskId}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
        {
            var result = await _todoListService.RemoveTagFromTaskAsync(taskId, tagId);
            if (!result) return NotFound($"Tag with ID {tagId} or Task with ID {taskId} not found.");
            return NoContent();
        }

        [HttpPost("tasks/{taskId}/comments")]
        public async Task<IActionResult> AddCommentToTask(int taskId, [FromBody] CommentDto commentDto)
        {
            var comment = await _todoListService.AddCommentToTaskAsync(taskId, commentDto);
            if (comment == null) return NotFound($"Task with ID {taskId} not found.");
            return Ok(comment);
        }

        [HttpGet("tasks/{taskId}/comments")]
        public async Task<IActionResult> GetCommentsByTaskId(int taskId)
        {
            var comments = await _todoListService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        [HttpPut("comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentDto commentDto)
        {
            var updatedComment = await _todoListService.UpdateCommentAsync(commentId, commentDto);
            if (updatedComment == null) return NotFound($"Comment with ID {commentId} not found.");
            return Ok(updatedComment);
        }

        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var result = await _todoListService.DeleteCommentAsync(commentId);
            if (!result) return NotFound($"Comment with ID {commentId} not found.");
            return NoContent();
        }
    }
}

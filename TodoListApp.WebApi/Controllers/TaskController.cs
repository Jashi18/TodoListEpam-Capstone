using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost("{todoListId}/tasks")]
        public async Task<IActionResult> AddTaskToTodoList(int todoListId, [FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedTask = await _taskService.AddTaskToTodoListAsync(todoListId, taskDto);
            if (addedTask == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetTaskById), new { taskId = addedTask.Id }, addedTask);
        }

        [HttpGet("tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            return Ok(task);
        }

        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByAssignedUserId(string userId)
        {
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);

            if (tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [HttpPut("tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDto taskDto)
        {
            var updatedTask = await _taskService.UpdateTaskAsync(taskId, taskDto);
            if (updatedTask == null)
            {
                return NotFound();
            }

            return Ok(updatedTask);
        }


        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(taskId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{taskId}/tags/{tagId}")]
        public async Task<IActionResult> AddTagToTask(int taskId, int tagId)
        {
            var success = await _taskService.AddTagToTaskAsync(taskId, tagId);
            if (!success)
            {
                return NotFound("Task or Tag not found.");
            }

            return Ok("Tag added to task successfully.");
        }

    }
}

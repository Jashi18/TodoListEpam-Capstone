using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITodoListService _todoListService;

        public TasksController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var taskDto = await _todoListService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }
            return View(taskDto);
        }
    }
}

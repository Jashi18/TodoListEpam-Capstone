using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Models;

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

        [HttpPost]
        public async Task<IActionResult> MarkCompleted(int id, bool isCompleted)
        {
            var taskDto = await _todoListService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }

            taskDto.IsCompleted = isCompleted;
            await _todoListService.UpdateTaskAsync(id, taskDto);

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public IActionResult Create(int todoListId)
        {
            var model = new CreateTaskViewModel { TodoListId = todoListId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taskDto = new TaskDto
                {
                    Title = model.Title,
                    Description = model.Description,
                    IsCompleted = model.IsCompleted,
                    TodoListId = model.TodoListId,
                    Deadline = model.Deadline
                };

                await _todoListService.AddTaskToTodoListAsync(model.TodoListId, taskDto);
                return RedirectToAction("Index", "TodoList", new { id = model.TodoListId });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var taskDto = await _todoListService.GetTaskByIdAsync(id);
            await _todoListService.DeleteTaskAsync(id);
            return RedirectToAction("Index", "TodoList", new { todoListId = taskDto.TodoListId });
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;
using TodoListApp.Services.WebApi;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskWebApiService _taskService;

        public TasksController(TaskWebApiService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var taskDto = await _taskService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }
            return View(taskDto);
        }

        [HttpPost]
        public async Task<IActionResult> MarkCompleted(int id, bool isCompleted)
        {
            var taskDto = await _taskService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }

            taskDto.IsCompleted = isCompleted;
            await _taskService.UpdateTaskAsync(taskDto);

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

                await _taskService.AddTaskToTodoListAsync(model.TodoListId, taskDto);
                return RedirectToAction("Index", "TodoList", new { id = model.TodoListId });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var taskDto = await _taskService.GetTaskByIdAsync(id);
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction("Index", "TodoList", new { todoListId = taskDto.TodoListId });
        }
    }
}
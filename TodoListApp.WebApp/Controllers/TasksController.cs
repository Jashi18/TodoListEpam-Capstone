using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoListApp.Services;
using TodoListApp.Services.WebApi;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskWebApiService _taskService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CommentWebApiService _commentService;
        private readonly TagWebApiService _tagService;

        public TasksController(TaskWebApiService taskService, 
            UserManager<IdentityUser> userManager, 
            CommentWebApiService commentService, 
            TagWebApiService tagService)
        {
            _taskService = taskService;
            _userManager = userManager;
            _commentService = commentService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var comments = await _commentService.GetCommentsByTaskIdAsync(id);

            var taskDto = await _taskService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }
            taskDto.Comments = comments.ToList();
            var users = _userManager.Users.Select(u => new { u.Id, u.UserName }).ToList();
            var allTags = await _tagService.GetAllTagsAsync();
            ViewBag.AllTags = new SelectList(allTags, "Id", "Name");
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            return View(taskDto);
        }




        [HttpGet]
        public async Task<IActionResult> Create(int todoListId)
        {
            var users = _userManager.Users.ToList();
            var model = new CreateTaskViewModel
            {
                TodoListId = todoListId,
                Users = new SelectList(users, "Id", "UserName")
            };
            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            model.Users = new SelectList(_userManager.Users.ToList(), "Id", "UserName");
            ModelState.Remove("Users");
            ModelState.Remove("SelectedTagIds");
            ModelState.Remove("Tags");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var taskDto = new TaskDto
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted,
                TodoListId = model.TodoListId,
                Deadline = model.Deadline,
                AssignedUserId = model.AssignedUserId
            };

            var newTaskDto = await _taskService.AddTaskToTodoListAsync(model.TodoListId, taskDto);

            return RedirectToAction("Index", "TodoList", new { id = model.TodoListId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var taskDto = await _taskService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }

            var users = _userManager.Users.ToList();
            ViewBag.Users = new SelectList(users, "Id", "UserName", taskDto.AssignedUserId);

            return View(taskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskDto taskDto)
        {
            ModelState.Remove("AssignedUserId");
            if (id != taskDto.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(taskDto);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalTaskDto = await _taskService.GetTaskByIdAsync(id);
                    if (originalTaskDto == null)
                    {
                        return NotFound();
                    }

                    taskDto.AssignedUserId = originalTaskDto.AssignedUserId;

                    await _taskService.UpdateTaskAsync(id, taskDto);
                }
                catch (Exception)
                {
                    return View(taskDto);
                }

                return RedirectToAction("Index", "TodoList");
            }

            return View(taskDto);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _taskService.DeleteTaskAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Task successfully deleted.";

            return RedirectToAction("Index", "TodoList");
        }




        [HttpGet]
        public async Task<IActionResult> AssignedToMe()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var tasks = await _taskService.GetTasksAssignedToUserAsync(user.Id);
            return View(tasks);
        }




        [HttpPost]
        public async Task<IActionResult> ReAssignTask(int taskId, string assignedUserId)
        {
            var taskDto = await _taskService.GetTaskByIdAsync(taskId);
            if (taskDto == null)
            {
                return NotFound();
            }

            taskDto.AssignedUserId = assignedUserId;
            var updatedTask = await _taskService.UpdateTaskAsync(taskId, taskDto);
            if (updatedTask == null)
            {
                return BadRequest("Unable to re-assign the task.");
            }

            return RedirectToAction("Details", new { id = taskId });
        }




        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string commentText)
        {
            var taskDto = await _taskService.GetTaskByIdAsync(taskId);
            if (taskDto == null)
            {
                return NotFound();
            }

            var userName = _userManager.GetUserName(User);

            var commentDto = new CommentDto
            {
                Text = commentText,
                TaskId = taskId,
                UserName = userName
            };

            await _commentService.AddCommentToTaskAsync(taskId, commentDto);

            return RedirectToAction("Details", new { id = taskId });
        }

        [HttpPost]
        public async Task<IActionResult> AddTagToTask(int taskId, int SelectedTagId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = taskId });
            }

            Console.WriteLine($"Task ID: {taskId}, Selected Tag ID: {SelectedTagId}");
            await _taskService.AddTagToTaskAsync(taskId, SelectedTagId);

            return RedirectToAction("Details", new { id = taskId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View();
            }
            await _tagService.RemoveTagFromTaskAsync(taskId, tagId);

            return RedirectToAction("Details", new { id = taskId });
        }
    }
}

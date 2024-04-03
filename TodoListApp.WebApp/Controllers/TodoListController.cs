using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ITodoListService _todoListService;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoListController(ITodoListService todoListService, UserManager<IdentityUser> userManager)
        {
            _todoListService = todoListService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var todoLists = await _todoListService.GetAllTodoListsAsync(userId);
            return View(todoLists);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            bool result = await _todoListService.DeleteTodoListAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoList(TodoListDto todoListDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdTodoList = await _todoListService.CreateTodoListAsync(todoListDto, userId);
            return View(createdTodoList);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateTodoListViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var todoListDto = new TodoListDto { Name = model.Name};
                var createdTodoList = await _todoListService.CreateTodoListAsync(todoListDto, userId);

                if (createdTodoList != null)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "An error occurred creating the todo list.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var todoList = await _todoListService.GetTodoListByIdAsync(id);
            if (todoList == null) return NotFound();

            return View(todoList);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TodoListDto todoListDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(todoListDto);
            }
            await _todoListService.UpdateTodoListAsync(id, todoListDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoListService.DeleteTodoListAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.Services;
using TodoListApp.Services.WebApi;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly TodoListWebApiService _todoListService;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoListController(TodoListWebApiService todoListService, UserManager<IdentityUser> userManager)
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

                var todoListDto = new TodoListDto
                {
                    Name = model.Name,
                    UserId = userId,
                };

                await _todoListService.CreateTodoListAsync(todoListDto);

                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var todoListDto = await _todoListService.GetTodoListByIdAsync(id);
            if (todoListDto == null)
            {
                return NotFound();
            }
            return View(todoListDto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoListDto todoListDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            todoListDto.UserId = userId;
            ModelState.Remove("UserId");
            if (id != todoListDto.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(todoListDto);
            }
            if (ModelState.IsValid)
            {
                bool success = await _todoListService.UpdateTodoListAsync(id, todoListDto);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while updating the todo list.");
                }
            }
            return View(todoListDto);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoListService.DeleteTodoListAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

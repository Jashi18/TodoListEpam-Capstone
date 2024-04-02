using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    public class TodoListController : Controller
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        public async Task<IActionResult> Index()
        {
            var todoLists = await _todoListService.GetAllTodoListsAsync();
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

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database;
using TodoListApp.Services.Database.Entities;

namespace TodoListApp.WebApp.Controllers
{
    public class TodoListController : Controller
    {
        private readonly TodoListDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoListController(TodoListDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserID = _userManager.GetUserId(User);
            var todoLists = await _context.TodoLists
                                           .Where(t => t.UserId == currentUserID)
                                           .ToListAsync();
            return View(todoLists);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodoList(string title)
        {
            var todoList = new TodoListEntity { Title = title, UserId = _userManager.GetUserId(User) };
            _context.Add(todoList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

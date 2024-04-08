using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database.Services
{
    public class TodoListDatabaseService : ITodoListService
    {
        private readonly TodoListDbContext _context;

        public TodoListDatabaseService(TodoListDbContext context)
        {
            _context = context;
        }

        public async Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto)
        {
            var todoList = new TodoListEntity { Name = todoListDto.Name, UserId = todoListDto.UserId };
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();
            return new TodoListDto { Id = todoList.Id, Name = todoList.Name };
        }

        public async Task<TodoListDto> GetTodoListByIdAsync(int id)
        {
            var todoList = await _context.TodoLists.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == id);
            if (todoList == null) return null;
            return new TodoListDto { 
                Id = todoList.Id, 
                Name = todoList.Name, 
                Tasks = todoList.Tasks.Select(t => new TaskDto { 
                    Id = t.Id, 
                    Title = t.Title, 
                    Description = t.Description, 
                    IsCompleted = t.IsCompleted, 
                    Deadline = t.Deadline }).ToList() };
        }

        public async Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync(string userId)
        {
            var todoLists = await _context.TodoLists
                .Where(t => t.UserId == userId)
    .Include(t => t.Tasks)
        .ThenInclude(task => task.Tags)
    .Include(t => t.Tasks)
        .ThenInclude(task => task.Comments)
    .ToListAsync();
            return todoLists.Select(tl => new TodoListDto
            {
                Id = tl.Id,
                Name = tl.Name,
                Tasks = tl.Tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    TodoListId = t.TodoListId,
                    Deadline = t.Deadline,
                    Tags = t.Tags.Select(tag => new TagDto { Id = tag.Id, Name = tag.Name }).ToList(),
                    Comments = t.Comments.Select(c => new CommentDto { 
                        Id = c.Id, 
                        Text = c.Text, 
                        CreatedAt = c.CreatedAt }).ToList()
                }).ToList()
            }).ToList();
        }

        public async Task<TodoListDto> UpdateTodoListAsync(int id, TodoListDto todoListDto)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null) return null;
            todoList.Name = todoListDto.Name;
            await _context.SaveChangesAsync();
            return todoListDto;
        }

        public async Task<bool> DeleteTodoListAsync(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null) return false;
            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
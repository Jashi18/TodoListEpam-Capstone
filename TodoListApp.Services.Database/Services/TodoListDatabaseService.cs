using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync()
        {
            var lists = await _context.TodoLists
                .Include(l => l.Tasks)
                .Select(list => new TodoListDto
                {
                    Id = list.Id,
                    Title = list.Title,
                    UserId = list.UserId,
                    Tasks = list.Tasks.Select(task => new TodoTaskDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        CreatedDate = task.CreatedDate,
                        DueTo = task.DueTo
                    }).ToList()
                }).ToListAsync();

            return lists;
        }

        public async Task<TodoListDto> GetTodoListByIdAsync(int id)
        {
            var list = await _context.TodoLists
                .Include(l => l.Tasks)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (list == null) return null;

            return new TodoListDto
            {
                Id = list.Id,
                Title = list.Title,
                UserId = list.UserId,
                Tasks = list.Tasks.Select(task => new TodoTaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreatedDate = task.CreatedDate,
                    DueTo = task.DueTo
                }).ToList()
            };
        }

        public async Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto)
        {
            var todoList = new TodoListEntity
            {
                Title = todoListDto.Title,
                UserId = todoListDto.UserId,
                Tasks = todoListDto.Tasks.Select(task => new TodoTaskEntity
                {
                    Title = task.Title,
                    Description = task.Description,
                    DueTo = task.DueTo
                }).ToList()
            };

            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            todoListDto.Id = todoList.Id; // Update the DTO with the new ID.
            return todoListDto;
        }

        public async Task UpdateTodoListAsync(TodoListDto todoListDto)
        {
            var todoList = await _context.TodoLists
                .Include(l => l.Tasks)
                .FirstOrDefaultAsync(l => l.Id == todoListDto.Id);

            if (todoList != null)
            {
                todoList.Title = todoListDto.Title;
                todoList.UserId = todoListDto.UserId;
                // For simplicity, not updating tasks here. You might want to handle task updates separately.

                _context.TodoLists.Update(todoList);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTodoListAsync(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList != null)
            {
                _context.TodoLists.Remove(todoList);
                await _context.SaveChangesAsync();
            }
        }
    }
}

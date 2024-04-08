using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database.Services
{
    public class TaskDatabaseService : ITaskService
    {
        private readonly TodoListDbContext _context;
        public TaskDatabaseService(TodoListDbContext context)
        {
            _context = context;
        }
        public async Task<TaskDto> AddTaskToTodoListAsync(int todoListId, TaskDto taskDto)
        {
            var todoList = await _context.TodoLists.FindAsync(todoListId);
            if (todoList == null) return null;

            var task = new TaskEntity
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                TodoListId = todoListId,
                Deadline = taskDto.Deadline,
                AssignedUserId = taskDto.AssignedUserId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                TodoListId = task.TodoListId,
                Deadline = task.Deadline,
                AssignedUserId = task.AssignedUserId
            };
        }

        public async Task<bool> AddTagToTaskAsync(int taskId, int tagId)
        {
            var task = await _context.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            var tag = await _context.Tags.FindAsync(tagId);

            if (task == null || tag == null) return false;

            task.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TaskDto> UpdateTaskAsync(int taskId, TaskDto taskDto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.IsCompleted = taskDto.IsCompleted;
            task.Deadline = taskDto.Deadline;
            task.AssignedUserId = taskDto.AssignedUserId;

            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                TodoListId = task.TodoListId,
                Deadline = task.Deadline,
                AssignedUserId = task.AssignedUserId
            };
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByAssignedUserIdAsync(string userId)
        {
            var tasks = await _context.Tasks
                                      .Where(task => task.AssignedUserId == userId)
                                      .Select(task => new TaskDto
                                      {
                                          Id = task.Id,
                                          Title = task.Title,
                                          Description = task.Description,
                                          IsCompleted = task.IsCompleted,
                                          Deadline = task.Deadline,
                                          TodoListId = task.TodoListId,
                                          AssignedUserId = task.AssignedUserId
                                      })
                                      .ToListAsync();

            return tasks;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(string userId)
        {
            return await _context.Tasks
                                 .Where(t => t.AssignedUserId == userId)
                                 .Include(t => t.Tags)
                                 .Include(t => t.Comments)
                                 .Select(t => new TaskDto
                                 {
                                     Id = t.Id,
                                     Title = t.Title,
                                     Description = t.Description,
                                     IsCompleted = t.IsCompleted,
                                     TodoListId = t.TodoListId,
                                     Deadline = t.Deadline,
                                     AssignedUserId = t.AssignedUserId,
                                 }).ToListAsync();
        }



        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId)
        {
            var taskEntity = await _context.Tasks
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (taskEntity == null) return null;

            var taskDto = new TaskDto
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                IsCompleted = taskEntity.IsCompleted,
                TodoListId = taskEntity.TodoListId,
                Deadline = taskEntity.Deadline,
                Tags = taskEntity.Tags.Select(tag => new TagDto { Id = tag.Id, Name = tag.Name }).ToList(),
                Comments = taskEntity.Comments.Select(comment => new CommentDto { 
                    Id = comment.Id, 
                    Text = comment.Text, 
                    CreatedAt = comment.CreatedAt 
                }).ToList()
            };

            return taskDto;
        }
    }
}

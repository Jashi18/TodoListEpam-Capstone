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

        public async Task<TodoListDto> CreateTodoListAsync(TodoListDto todoListDto, string userId)
        {
            var todoList = new TodoListEntity { Name = todoListDto.Name, UserId = userId };
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();
            return new TodoListDto { Id = todoList.Id, Name = todoList.Name };
        }

        public async Task<TodoListDto> GetTodoListByIdAsync(int id)
        {
            var todoList = await _context.TodoLists.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == id);
            if (todoList == null) return null;
            return new TodoListDto { Id = todoList.Id, Name = todoList.Name, Tasks = todoList.Tasks.Select(t => new TaskDto { Id = t.Id, Title = t.Title, Description = t.Description, IsCompleted = t.IsCompleted, Deadline=t.Deadline }).ToList() };
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
                    Comments = t.Comments.Select(c => new CommentDto { Id = c.Id, Text = c.Text, CreatedAt = c.CreatedAt }).ToList()
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

        public async Task<TaskDto> AddTaskToTodoListAsync(int todoListId, TaskDto taskDto)
        {
            var todoList = await _context.TodoLists.FindAsync(todoListId);
            if (todoList == null) return null;
            var task = new TaskEntity { Title = taskDto.Title, Description = taskDto.Description, IsCompleted = taskDto.IsCompleted, TodoListId = todoListId, Deadline = taskDto.Deadline };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return new TaskDto { Id = task.Id, Title = task.Title, Description = task.Description, IsCompleted = task.IsCompleted, TodoListId = task.TodoListId, Deadline = taskDto.Deadline };


        }

        public async Task<TaskDto> UpdateTaskAsync(int taskId, TaskDto taskDto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.IsCompleted = taskDto.IsCompleted;
            await _context.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                TodoListId = task.TodoListId
            };
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<TagDto> AddTagToTaskAsync(int taskId, TagDto tagDto)
        {
            var task = await _context.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return null;

            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name) ?? new TagEntity { Name = tagDto.Name };
            if (!task.Tags.Contains(tag))
            {
                task.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return new TagDto { Id = tag.Id, Name = tag.Name };
        }

        public async Task<IEnumerable<TagDto>> GetTagsByTaskIdAsync(int taskId)
        {
            var tags = await _context.Tasks
                .Where(t => t.Id == taskId)
                .SelectMany(t => t.Tags)
                .Select(tag => new TagDto { Id = tag.Id, Name = tag.Name })
                .ToListAsync();

            return tags;
        }

        public async Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId)
        {
            var task = await _context.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            var tag = task?.Tags.FirstOrDefault(t => t.Id == tagId);
            if (task == null || tag == null) return false;

            task.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CommentDto> AddCommentToTaskAsync(int taskId, CommentDto commentDto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return null;

            var comment = new CommentEntity { Text = commentDto.Text, TaskId = taskId };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return new CommentDto { Id = comment.Id, Text = comment.Text, CreatedAt = comment.CreatedAt };
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(int taskId)
        {
            var comments = await _context.Comments
                .Where(c => c.TaskId == taskId)
                .Select(c => new CommentDto { Id = c.Id, Text = c.Text, CreatedAt = c.CreatedAt })
                .ToListAsync();

            return comments;
        }

        public async Task<CommentDto> UpdateCommentAsync(int commentId, CommentDto commentDto)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return null;

            comment.Text = commentDto.Text;
            await _context.SaveChangesAsync();

            return new CommentDto { Id = comment.Id, Text = comment.Text, CreatedAt = comment.CreatedAt };
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
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
                Comments = taskEntity.Comments.Select(comment => new CommentDto { Id = comment.Id, Text = comment.Text, CreatedAt = comment.CreatedAt }).ToList()
            };

            return taskDto;
        }
    }
}

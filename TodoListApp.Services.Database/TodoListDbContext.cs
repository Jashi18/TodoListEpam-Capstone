using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database
{
    public class TodoListDbContext : IdentityDbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
    : base(options)
        {
        }

        public DbSet<TodoListEntity> TodoLists { get; set; }
        public DbSet<TodoTaskEntity> TodoTasks { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace TodoListApp.Services.Database.Entities
{
    public class TodoListEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}

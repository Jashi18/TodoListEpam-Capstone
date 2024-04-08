
namespace TodoListApp.Services.Database.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TaskId { get; set; }
        public TaskEntity Task { get; set; }
        public string UserName { get; set; }
    }
}

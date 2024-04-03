namespace TodoListApp.Services.Database.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Deadline { get; set; }
        public int TodoListId { get; set; }
        public TodoListEntity TodoList { get; set; }
        public ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();
        public ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
    }
}

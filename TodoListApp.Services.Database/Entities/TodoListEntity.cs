namespace TodoListApp.Services.Database.Entities
{
    public class TodoListEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public ICollection<TodoTaskEntity> Tasks { get; set; }
    }
}

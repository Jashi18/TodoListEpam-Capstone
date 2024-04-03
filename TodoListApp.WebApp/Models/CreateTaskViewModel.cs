namespace TodoListApp.WebApp.Models
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int TodoListId { get; set; }
        public DateTime Deadline { get; set; }
    }
}

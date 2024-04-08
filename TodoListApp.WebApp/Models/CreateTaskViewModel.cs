using Microsoft.AspNetCore.Mvc.Rendering;

namespace TodoListApp.WebApp.Models
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int TodoListId { get; set; }
        public DateTime Deadline { get; set; }
        public int[] SelectedTagIds { get; set; }
        public SelectList Tags { get; set; }
        public SelectList Users { get; set; }
        public string AssignedUserId { get; set; }

    }
}

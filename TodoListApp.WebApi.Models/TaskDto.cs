using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.WebApi.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime Deadline { get; set; }
        public int TodoListId { get; set; }
        public int? SelectedTagId { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public string AssignedUserId { get; set; } = string.Empty;
    }
}

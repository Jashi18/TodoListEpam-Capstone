﻿namespace TodoListApp.WebApi.Models
{
    public class TodoTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime DueTo { get; set; }
    }
}

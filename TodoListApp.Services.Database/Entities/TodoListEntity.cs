﻿namespace TodoListApp.Services.Database.Entities
{
    public class TodoListEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.WebApi.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int TaskId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserName { get; set; }
    }
}

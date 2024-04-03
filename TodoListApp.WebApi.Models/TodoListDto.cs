using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.WebApi.Models
{
    public class TodoListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}

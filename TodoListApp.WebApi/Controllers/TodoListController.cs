using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        // GET: api/TodoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListDto>>> GetAllTodoLists()
        {
            var todoLists = await _todoListService.GetAllTodoListsAsync();
            return Ok(todoLists);
        }

        // GET: api/TodoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListDto>> GetTodoList(int id)
        {
            var todoList = await _todoListService.GetTodoListByIdAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            return Ok(todoList);
        }

        // POST: api/TodoList
        [HttpPost]
        public async Task<ActionResult<TodoListDto>> CreateTodoList([FromBody] TodoListDto todoListDto)
        {
            var createdTodoList = await _todoListService.CreateTodoListAsync(todoListDto);
            return CreatedAtAction(nameof(GetTodoList), new { id = createdTodoList.Id }, createdTodoList);
        }

        // PUT: api/TodoList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoList(int id, [FromBody] TodoListDto todoListDto)
        {
            if (id != todoListDto.Id)
            {
                return BadRequest("Todo List ID mismatch");
            }

            await _todoListService.UpdateTodoListAsync(todoListDto);

            return NoContent();
        }

        // DELETE: api/TodoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            await _todoListService.DeleteTodoListAsync(id);
            return NoContent();
        }
    }
}

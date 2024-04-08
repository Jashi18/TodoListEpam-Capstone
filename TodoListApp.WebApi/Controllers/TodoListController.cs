using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [HttpPost]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoListDto todoListDto)
        {
            var todoList = new TodoListDto { 
                Id = todoListDto.Id, 
                Name = todoListDto.Name, 
                UserId = todoListDto.UserId 
            };
            await _todoListService.CreateTodoListAsync(todoList);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoList(int id)
        {
            var todoList = await _todoListService.GetTodoListByIdAsync(id);
            if (todoList == null) return NotFound();
            return Ok(todoList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodoLists(string userId)
        {
            var todoLists = await _todoListService.GetAllTodoListsAsync(userId);
            return Ok(todoLists);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoList(int id, [FromBody] TodoListDto todoListDto)
        {
            var updatedTodoList = await _todoListService.UpdateTodoListAsync(id, todoListDto);
            if (updatedTodoList == null) return NotFound();
            return Ok(updatedTodoList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            var result = await _todoListService.DeleteTodoListAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
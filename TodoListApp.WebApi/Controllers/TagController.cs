using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("tags/{id}")]
        public async Task<ActionResult<TagDto>> GetTagById(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [HttpGet("tags")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }


        [HttpPost("tags")]
        public async Task<IActionResult> CreateTag([FromBody] TagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTag = await _tagService.CreateTagAsync(tagDto);
                if (createdTag == null)
                {
                    return BadRequest("Could not create the tag.");
                }

                return CreatedAtAction(nameof(GetTagById), new { id = createdTag.Id }, createdTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("tags/{tagId}")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            var success = await _tagService.DeleteTagAsync(tagId);
            if (!success)
            {
                return NotFound($"Tag with ID {tagId} not found.");
            }

            return NoContent();
        }


        [HttpGet("tasks/{taskId}/tags")]
        public async Task<IActionResult> GetTagsByTaskId(int taskId)
        {
            var tags = await _tagService.GetTagsByTaskIdAsync(taskId);
            return Ok(tags);
        }

        [HttpDelete("tasks/{taskId}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
        {
            var result = await _tagService.RemoveTagFromTaskAsync(taskId, tagId);
            if (!result) return NotFound($"Tag with ID {tagId} or Task with ID {taskId} not found.");
            return NoContent();
        }
    }
}

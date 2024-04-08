using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database.Services
{
    public class TagDatabaseService : ITagService
    {
        private readonly TodoListDbContext _context;

        public TagDatabaseService(TodoListDbContext context)
        {
            _context = context;
        }
        public async Task<TagDto> AddTagToTaskAsync(int taskId, TagDto tagDto)
        {
            var task = await _context.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return null;

            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name) ?? new TagEntity { Name = tagDto.Name };
            if (!task.Tags.Contains(tag))
            {
                task.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return new TagDto { Id = tag.Id, Name = tag.Name };
        }

        public async Task<TagDto> CreateTagAsync(TagDto tagDto)
        {
            var tag = new TagEntity
            {
                Name = tagDto.Name
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return null;
            }

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }



        public async Task<IEnumerable<TagDto>> GetTagsByTaskIdAsync(int taskId)
        {
            var tags = await _context.Tasks
                .Where(t => t.Id == taskId)
                .SelectMany(t => t.Tags)
                .Select(tag => new TagDto { Id = tag.Id, Name = tag.Name })
                .ToListAsync();
            return tags;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _context.Tags
                .Select(tag => new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name
                })
                .ToListAsync();

            return tags;
        }


        public async Task<bool> RemoveTagFromTaskAsync(int taskId, int tagId)
        {
            var task = await _context.Tasks.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            var tag = task?.Tags.FirstOrDefault(t => t.Id == tagId);
            if (task == null || tag == null) return false;

            task.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            _context.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

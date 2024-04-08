using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class TagController : Controller
    {
        private readonly TagWebApiService _tagService;

        public TagController(TagWebApiService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(tagDto);
            }
            if (ModelState.IsValid)
            {
                await _tagService.CreateTagAsync(tagDto);
                return RedirectToAction(nameof(Index));
            }
            return View(tagDto);
        }
    }
}

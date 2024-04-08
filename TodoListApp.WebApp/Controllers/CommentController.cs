using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.Services.WebApi;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentWebApiService _commentService;

        public CommentController(CommentWebApiService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CommentDto commentDto)
        {
            if (id != commentDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _commentService.UpdateCommentAsync(commentDto);
                if (!success)
                {
                    // Handle failure
                    ModelState.AddModelError("", "An error occurred while updating the comment.");
                    return View(commentDto);
                }

                // Redirect after successful update, adjust as needed
                return RedirectToAction("Details", "Tasks", new { id = commentDto.TaskId });
            }

            return View(commentDto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _commentService.DeleteCommentAsync(id);
            if (!success)
            {
                // Handle failure
                return NotFound();
            }

            // Redirect after successful deletion, adjust as needed
            return RedirectToAction(nameof(Index)); // Assuming 'Index' lists the tasks or comments
        }


    }
}

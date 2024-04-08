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
            ModelState.Remove("UserName");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(commentDto);
            }

            if (ModelState.IsValid)
            {
                var existingComment = await _commentService.GetCommentByIdAsync(id);
                if (existingComment == null)
                {
                    return NotFound($"Comment with ID {id} not found.");
                }

                commentDto.UserName = existingComment.UserName;

                var success = await _commentService.UpdateCommentAsync(commentDto);
                if (!success)
                {
                    ModelState.AddModelError("", "An error occurred while updating the comment.");
                    return View(commentDto);
                }

                return RedirectToAction("Details", "Tasks", new { id = commentDto.TaskId });
            }

            return View(commentDto);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            bool success = await _commentService.DeleteCommentAsync(id);
            if (!success)
            {
                return NotFound($"Comment with ID {id} not found.");
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "TodoList");
        }
    }
}

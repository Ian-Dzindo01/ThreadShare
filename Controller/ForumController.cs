using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreadShare.DTOs.Entites;
using ThreadShare.Service.Interfaces;
using ThreadShare.Models;
using Microsoft.AspNetCore.Authorization;


namespace Controllers.Forums
{
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            string name = formCollection["Name"];
            string description = formCollection["Description"];

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError("", "Both Name and Description are required");
                return View();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ForumViewModel forumViewModel = new ForumViewModel
            {
                Name = name,
                Description = description,
                UserId = userId
            };

            await _forumService.CreateForum(forumViewModel);
            return Redirect("~/");
        }

        // GET
        public IActionResult Delete()
        {
            return View();
        }

        // POST
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Delete(int forumId)
        {
            Console.WriteLine($"Forum Id is {forumId}");
            var forum = await _forumService.GetForumById(forumId);
            if (forum == null)
            {
                return NotFound();
            }

            await _forumService.DeleteForum(forumId);
            return Redirect("~/");
        }
    }
}

    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        Forum forum = await _forumService.GetForumById(id.Value);

    //        if (forum == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(forum);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description")] Forum forum)
    //    {
    //        if (id != forum.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            await _forumService.UpdateForum(forum, id);
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(forum);
    //    }
    //}
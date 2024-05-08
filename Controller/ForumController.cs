using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ThreadShare.DTOs.Data_Transfer;
using ThreadShare.DTOs.Entites;
using ThreadShare.Models;
using ThreadShare.Service.Interfaces;

namespace Controllers.Forums
{
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        // GET: /Forum/Create
        public IActionResult Create()
        {
            return View();
        }

        // The Name field is required.
        // The Description field is required.
        // POST: /Forum/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description")] ForumDTO forumDTO)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ForumViewModel forumViewModel = new ForumViewModel
                {
                    Name = forumDTO.Name,
                    Description = forumDTO.Description,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                await _forumService.CreateForum(forumViewModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach(var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage.ToString());
                }
                return View();
            }
        }

        //// GET: /Forum/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Forum forum = await _forumService.GetForumById(id.Value);
        //    if (forum == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(forum);
        //}

        //// POST: /Forum/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description")] Forum forum)
        //{
        //    if (id != forum.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        await _forumService.UpdateForum(forum, id);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(forum);
        //}
    }
}
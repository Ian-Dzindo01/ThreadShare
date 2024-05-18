﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ThreadShare.DTOs.Entites;
using ThreadShare.Models;
using ThreadShare.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ThreadShare.Service.Implementations;

namespace Controllers.Posts
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IForumService _forumService;

        public PostController(IPostService postService, IForumService forumService)
        {
            _postService = postService;
            _forumService = forumService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    List<Post> posts = await _postService.GetAllPosts();
        //    return View(posts);
        //}

        // GET: /Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Post/Create
        // Only respond to POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            string title = formCollection["Title"];
            string body = formCollection["Body"];
            string forumIdString = formCollection["ForumId"];

            int forumId;
            // Convert forumId to int
            if (!int.TryParse(forumIdString, out forumId))
            {
                ModelState.AddModelError("ForumId", "Invalid Forum ID");
            }

            if (!await _forumService.ForumExists(forumId))
            {
                ModelState.AddModelError("ForumId", "Forum ID does not exist");
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
            {
                ModelState.AddModelError("", "Both Title and Body are required");
                return View();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PostViewModel postViewModel = new PostViewModel
            {
                Title = title,
                Body = body,
                UserId = userId,
                ForumId = forumId
            };

            await _postService.CreatePost(postViewModel);
            return Redirect("~/");
        }
    }
}

//// GET: /Post/Edit/5
//public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            Post post = await _postService.GetPostById(id.Value);
//            if (post == null)
//            {
//                return NotFound();
//            }
//            return View(post);
//        }

        // POST: /Post/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id, Title, Content")] Post post)
        //{
        //    if (id != post.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        await _postService.UpdatePost(post, id);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(post);
        //}

        //// GET: /Post/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Post post = await _postService.GetPostById(id.Value);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        //// POST: /Post/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    await _postService.DeletePost(id);
        //    return RedirectToAction(nameof(Index));
        //}
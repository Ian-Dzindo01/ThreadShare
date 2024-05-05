using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThreadShare.DTOs.Entites;
using ThreadShare.Models;
using ThreadShare.Service.Interfaces;

namespace YourNamespace.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
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
        // Prevent CSRF attacks
        [ValidateAntiForgeryToken]
        // Convert to PostViewModel
        public async Task<IActionResult> Create([Bind("Title, Content")] PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                await _postService.CreatePost(postViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(postViewModel);
        }


        // GET: /Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post post = await _postService.GetPostById(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

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
    }
}
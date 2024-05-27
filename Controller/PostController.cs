using System.Security.Claims;
using ThreadShare.DTOs.Entites;
using ThreadShare.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        // GET: /Post/Create
        public async Task<IActionResult> Create()
        {
            var forums = await _forumService.GetAllForums();

            ViewBag.Forums = forums;

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
            string forumIdStr = formCollection["ForumName"];

            Console.WriteLine(forumIdStr);

            if (string.IsNullOrEmpty(forumIdStr) || !int.TryParse(forumIdStr, out int forumId))
            {
                ModelState.AddModelError("ForumId", "Invalid Forum ID");
                return View();
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
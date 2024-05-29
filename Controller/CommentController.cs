
using System.Security.Claims;
using ThreadShare.DTOs.Entites;
using ThreadShare.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Controllers.Comments
{
    public class CommentController : Controller
    {
        private readonly IPostService _postService;
        private readonly IForumService _forumService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public CommentController(IPostService postService, IForumService forumService,
                              IUserService userService, ICommentService commentService)
        {
            _postService = postService;
            _forumService = forumService;
            _userService = userService;
            _commentService = commentService;
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
        [ValidateAntiForgeryToken, HttpPost, Authorize]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            string body = formCollection["Body"];
            string forumIdStr = formCollection["ForumName"];
            string postIdStr = formCollection["PostName"];

            if (string.IsNullOrEmpty(forumIdStr) || !int.TryParse(forumIdStr, out int forumId))
            {
                ModelState.AddModelError("ForumId", "Invalid Forum ID");
                return View();
            }

            if (string.IsNullOrEmpty(forumIdStr) || !int.TryParse(forumIdStr, out int postId))
            {
                ModelState.AddModelError("ForumId", "Invalid Forum ID");
                return View();
            }


            if (string.IsNullOrWhiteSpace(body))
            {
                ModelState.AddModelError("", "Both Title and Body are required");
                return View();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            CommentViewModel commentViewModel = new CommentViewModel
            {
                Body = body,
                UserId = userId,
                ForumId = forumId
            };

            await _commentService.CreateComment(commentViewModel);

            // Redirect back to the post here!
            return Redirect("~/");
        }

        // GET: /Post/Details/2
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(post.UserId);
            var forum = await _forumService.GetForumById(post.ForumId);

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                Forum = forum,
                Username = user.Username
            };

            return View(viewModel);
        }
    }
}

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
        //// GET: /Post/Create
        [HttpGet]
        public async Task<IActionResult> Create(int postId, int forumId)
        {
            ViewData["PostId"] = postId;
            ViewData["ForumId"] = forumId;

            return View();
        }

        [ValidateAntiForgeryToken, HttpPost, Authorize]
        public async Task<IActionResult> Create(string body, int forumId, int postId)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                ModelState.AddModelError("", "Comment body is required");
                return View();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            CommentViewModel commentViewModel = new CommentViewModel
            {
                Body = body,
                UserId = userId,
                ForumId = forumId,
                PostId = postId
            };

            await _commentService.CreateComment(commentViewModel);

            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
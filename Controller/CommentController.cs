using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreadShare.DTOs.Entites;
using ThreadShare.Service.Interfaces;

namespace Controllers.Comments
{
    /// <summary>
    /// Handles comment-related actions within the application.
    /// </summary>
    public class CommentController : Controller
    {
        private readonly IPostService _postService;
        private readonly IForumService _forumService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentController"/> class.
        /// </summary>
        /// <param name="postService">The service for managing posts.</param>
        /// <param name="forumService">The service for managing forums.</param>
        /// <param name="userService">The service for managing users.</param>
        /// <param name="commentService">The service for managing comments.</param>
        public CommentController(IPostService postService, IForumService forumService,
                                  IUserService userService, ICommentService commentService)
        {
            _postService = postService;
            _forumService = forumService;
            _userService = userService;
            _commentService = commentService;
        }

        /// <summary>
        /// Displays the page to create a new comment for a specific post and forum.
        /// </summary>
        /// <param name="postId">The ID of the post to comment on.</param>
        /// <param name="forumId">The ID of the forum containing the post.</param>
        /// <returns>The view for creating a new comment.</returns>
        [HttpGet]
        public async Task<IActionResult> Create(int postId, int forumId)
        {
            ViewData["PostId"] = postId;
            ViewData["ForumId"] = forumId;

            return View();
        }

        /// <summary>
        /// Submits a new comment for a specific post in a forum.
        /// </summary>
        /// <param name="body">The body of the comment.</param>
        /// <param name="forumId">The ID of the forum containing the post.</param>
        /// <param name="postId">The ID of the post being commented on.</param>
        /// <returns>Redirects to the post details view if successful; otherwise, re-displays the create view with errors.</returns>
        /// <response code="302">Redirects to the post details page on successful creation.</response>
        /// <response code="400">Returns the create view with validation errors if the input is invalid.</response>
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreadShare.DTOs.Entites;
using ThreadShare.Service.Interfaces;

namespace ThreadShare.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IForumService _forumService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public PostController(IPostService postService, IForumService forumService,
                              IUserService userService, ICommentService commentService)
        {
            _postService = postService;
            _forumService = forumService;
            _userService = userService;
            _commentService = commentService;
        }

        /// <summary>
        /// Creates a new post in a forum.
        /// </summary>
        /// <param name="Title">The title of the post.</param>
        /// <param name="Body">The body/content of the post.</param>
        /// <param name="ForumId">The ID of the forum where the post will be created.</param>
        /// <returns>Redirects to the homepage on success or returns an error message if validation fails.</returns>
        /// <response code="302">Redirects to the homepage on successful creation.</response>
        /// <response code="400">If the Title or Body is invalid.</response>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(string Title, string Body, int ForumId)
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Body))
            {
                return BadRequest("Both Title and Body are required.");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postViewModel = new PostViewModel
            {
                Title = Title,
                Body = Body,
                UserId = userId,
                ForumId = ForumId
            };

            await _postService.CreatePost(postViewModel);

            return Redirect("~/");
        }

        /// <summary>
        /// Displays the page for creating a new post.
        /// </summary>
        /// <returns>The view for creating a new post with a list of available forums.</returns>
        /// <response code="200">Returns the create post view with forum data.</response>
        /// <response code="404">If no forums are available to post in.</response>
        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var forums = await _forumService.GetAllForums();
            if (forums == null || !forums.Any())
            {
                return NotFound("No forums available to post in.");
            }

            ViewBag.Forums = forums.Select(forum => new ForumViewModel
            {
                Id = forum.Id,
                Name = forum.Name,
                Description = forum.Description,
                UserId = forum.UserId
            }).ToList();

            return View();
        }

        /// <summary>
        /// Retrieves the details of a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post to retrieve.</param>
        /// <returns>The view displaying the post details along with its forum, user, and comments.</returns>
        /// <response code="200">Returns the details of the post.</response>
        /// <response code="404">If the post is not found.</response>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(post.UserId);
            var forum = await _forumService.GetForumById(post.ForumId);
            var comments = await _commentService.GetCommentsForPost(post.Id);

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                Forum = forum,
                Comments = comments,
                Username = user.UserName
            };

            return View(viewModel);
        }
    }
}

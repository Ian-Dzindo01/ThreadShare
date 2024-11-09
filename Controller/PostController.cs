using System.Security.Claims;
using ThreadShare.DTOs.Entites;
using ThreadShare.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ThreadShare.Models;

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
        /// Creates a new post.
        /// </summary>
        /// <param name="postViewModel">The post details.</param>
        /// <returns>Returns a redirect to the homepage if successful.</returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PostViewModel postViewModel)
        {
            if (postViewModel == null || string.IsNullOrWhiteSpace(postViewModel.Title) || string.IsNullOrWhiteSpace(postViewModel.Body))
            {
                return BadRequest("Both Title and Body are required.");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            postViewModel.UserId = userId;

            await _postService.CreatePost(postViewModel);

            return Ok("Post created successfully.");
        }

        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var forums = await _forumService.GetAllForums();
            if (forums == null || !forums.Any())
            {
                return NotFound("No forums available to post in.");
            }

            var viewModel = new CreatePostViewModel
            {
                Forums = forums.Select(forum => new ForumViewModel
                {
                    Name = forum.Name,  
                    Description = forum.Description
                }).ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Gets the details of a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>Returns the post details.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(post.UserId);
            Console.WriteLine("Retrieving User successful");
            Console.WriteLine(post.UserId);
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

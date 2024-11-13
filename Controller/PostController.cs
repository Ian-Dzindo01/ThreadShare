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
        public async Task<IActionResult> Create(string Title, string Body, int ForumId)
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Body))
            {
                return BadRequest("Both Title and Body are required.");
            }

            //int? forumId = await _forumService.GetForumIdByName(Title); 
            //if (forumId == null)
            //{
            //    return NotFound("The specified forum does not exist.");
            //}

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

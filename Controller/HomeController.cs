using Microsoft.AspNetCore.Mvc;
using ThreadShare.Models;
using ThreadShare.Service.Interfaces;

namespace ThreadShare.Controllers
{

    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IForumService _forumService;

        public HomeController(IPostService postService, IForumService forumService)
        {
            _postService = postService;
            _forumService = forumService;
        }

        /// <summary>
        /// Displays the home page with the newest posts and available forums.
        /// </summary>
        /// <returns>A view displaying the newest posts and forums.</returns>
        /// <response code="200">Returns the home page view with data.</response>
        public async Task<IActionResult> Index()
        {
            var newestPosts = await _postService.GetNewestPosts();
            var forums = await _forumService.GetAllForums();

            var viewModel = new HomePageViewModel
            {
                NewestPosts = newestPosts,
                Forums = forums
            };

            return View(viewModel);
        }
    }
}

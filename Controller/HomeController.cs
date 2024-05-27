using Microsoft.AspNetCore.Mvc;
using ThreadShare.Service.Interfaces;
using ThreadShare.Models; 

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

using Microsoft.AspNetCore.Mvc;

namespace ThreadShare.Controller
{
    public class ForumController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

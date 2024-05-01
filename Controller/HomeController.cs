using Microsoft.AspNetCore.Mvc;

namespace ThreadShare.Controller
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

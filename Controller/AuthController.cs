using Microsoft.AspNetCore.Mvc;


namespace ThreadShare.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

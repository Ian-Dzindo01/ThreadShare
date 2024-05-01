using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThreadShare.DTOs;
using ThreadShare.Models;

namespace ThreadShare.Controller 
{
    public class PostController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

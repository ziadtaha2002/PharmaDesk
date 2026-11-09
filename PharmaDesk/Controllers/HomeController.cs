using Microsoft.AspNetCore.Mvc;

namespace PharmaDesk.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Greeting"] = "Good morning, Admin";
            ViewData["Title"] = "Dashboard";
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace SEP490_G74_RJMS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
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

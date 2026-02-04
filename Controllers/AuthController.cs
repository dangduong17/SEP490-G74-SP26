using Microsoft.AspNetCore.Mvc;

namespace SEP490_G74_RJMS.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public IActionResult LoginPost( /* login model */
        )
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        public IActionResult RegisterPost( /* register model */
        )
        {
            return RedirectToAction("Login");
        }

        // POST: Auth/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

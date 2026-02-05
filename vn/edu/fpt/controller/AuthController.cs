using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    public class AuthController : Controller
    {
        // Login - Allow user login using email and password
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // TODO: Implement login logic - 2 fields select (80 LOC)
            return RedirectToAction("Index", "Home");
        }

        // Register - Allow guest to register new account
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // TODO: Implement register logic - 4 fields insert (120 LOC)
            return RedirectToAction("Login");
        }

        // Logout - Allow user to logout from system
        public IActionResult Logout()
        {
            // TODO: Implement logout logic - no DB change (40 LOC)
            return RedirectToAction("Login");
        }

        // ChangePassword - Allow user to update account password
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(
            string currentPassword,
            string newPassword,
            string confirmPassword
        )
        {
            // TODO: Implement change password logic - 3 fields update (90 LOC)
            return View();
        }
    }
}

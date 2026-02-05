using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.controller
{
    public class AdminController : Controller
    {
        // AdminDashboard - Display system overview dashboard
        public IActionResult AdminDashboard()
        {
            // TODO: Implement admin dashboard logic - summary select (150 LOC)
            return View();
        }

        // UserList - Display list of system users
        public IActionResult UserList()
        {
            // TODO: Implement user list logic - select (120 LOC)
            return View();
        }

        // CreateUser - Allow admin to create new user
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            // TODO: Implement create user logic - insert (140 LOC)
            return RedirectToAction("UserList");
        }

        // EditUser - Allow admin to edit user information
        public IActionResult EditUser(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            // TODO: Implement edit user logic - update (130 LOC)
            return RedirectToAction("UserList");
        }

        // ActivateDeactivateUser - Update user account status
        [HttpPost]
        public IActionResult ActivateDeactivateUser(int userId, bool isActive)
        {
            // TODO: Implement activate/deactivate user logic - update (110 LOC)
            return RedirectToAction("UserList");
        }
    }
}

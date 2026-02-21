using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using vn.edu.fpt.dto;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.controller
{
    [Authorize(Roles = "Recruiter")]
    public class RecruiterController : Controller
    {
        private readonly IRecruiterService _recruiterService;

        public RecruiterController(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }

        [HttpGet]
        public async Task<IActionResult> RecruiterDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Login", "Auth");

            var model = await _recruiterService.GetDashboardAsync(userId);
            if (model == null) return RedirectToAction("AccessDenied", "Auth");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Login", "Auth");

            var model = await _recruiterService.GetProfileAsync(userId);
            if (model == null) return RedirectToAction("AccessDenied", "Auth");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(RecruiterProfileUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return RedirectToAction("Login", "Auth");

            var result = await _recruiterService.UpdateProfileAsync(userId, model);
            if (!result.Succeeded)
            {
                if (result.NotFound) return RedirectToAction("AccessDenied", "Auth");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key ?? string.Empty, error.Message);
                }

                return View(model);
            }

            TempData["SuccessToast"] = "Cap nhat ho so nha tuyen dung thanh cong.";
            return RedirectToAction(nameof(RecruiterDashboard));
        }

        [HttpGet]
        public IActionResult JobPostingList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateJob()
        {
            return View();
        }
    }
}

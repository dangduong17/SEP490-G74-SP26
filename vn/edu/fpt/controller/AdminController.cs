using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.dto;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.controller
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _adminService.GetDashboardAsync();
            return View("AdminDashboard", model);
        }

        public async Task<IActionResult> UserList(string? keyword, string? role, string? status, int page = 1, int pageSize = 10)
        {
            var model = await _adminService.GetUserListAsync(keyword, role, status, page, pageSize);
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateAdmin()
        {
            ViewData["Title"] = "Tạo tài khoản quản trị";
            return View(new AdminCreateAdminViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(AdminCreateAdminViewModel model)
        {
            ViewData["Title"] = "Tạo tài khoản quản trị";
            if (!ModelState.IsValid) return View(model);

            var result = await _adminService.CreateAdminAsync(model);
            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                return View(model);
            }

            TempData["Success"] = "Tạo tài khoản admin thành công.";
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public IActionResult CreateCandidate()
        {
            ViewData["Title"] = "Tạo ứng viên";
            return View(new AdminCreateCandidateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCandidate(AdminCreateCandidateViewModel model)
        {
            ViewData["Title"] = "Tạo ứng viên";
            if (!ModelState.IsValid) return View(model);

            var result = await _adminService.CreateCandidateAsync(model);
            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                return View(model);
            }

            TempData["Success"] = "Tạo ứng viên thành công.";
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public IActionResult CreateRecruiter()
        {
            ViewData["Title"] = "Tạo nhà tuyển dụng";
            return View(new AdminCreateRecruiterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecruiter(AdminCreateRecruiterViewModel model)
        {
            ViewData["Title"] = "Tạo nhà tuyển dụng";
            if (!ModelState.IsValid) return View(model);

            var result = await _adminService.CreateRecruiterAsync(model);
            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                return View(model);
            }

            TempData["Success"] = "Tạo nhà tuyển dụng thành công.";
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var model = await _adminService.GetUpdateUserAsync(id);
            if (model == null) return NotFound();

            ViewData["Title"] = "Cập nhật người dùng";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(AdminUpdateUserViewModel model)
        {
            ViewData["Title"] = "Cập nhật người dùng";
            if (!ModelState.IsValid) return View(model);

            var result = await _adminService.UpdateUserAsync(model);
            if (!result.Succeeded)
            {
                if (result.NotFound) return NotFound();

                AddErrorsToModelState(result);
                return View(model);
            }

            TempData["Success"] = "Cập nhật người dùng thành công.";
            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _adminService.SoftDeleteUserAsync(id);
            if (!result.Succeeded)
            {
                if (result.NotFound) return NotFound();
                TempData["Error"] = result.Errors.FirstOrDefault()?.Message ?? "Thao tác thất bại.";
                return RedirectToAction(nameof(UserList));
            }

            TempData["Success"] = "Đã chuyển trạng thái người dùng sang ngưng hoạt động.";
            return RedirectToAction(nameof(UserList));
        }

        private void AddErrorsToModelState(ServiceResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Key ?? string.Empty, error.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.controller
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(IAuthService authService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterRecruiter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRecruiter(RegisterDto registerDto)
        {
            return await Register(registerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return View(registerDto);

            
            
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { email = registerDto.Email }, Request.Scheme) ?? "";
            
           
            
            var result = await _authService.RegisterAsync(registerDto, confirmationLink);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Đăng ký thất bại.");
                return View(registerDto);
            }

            TempData["SuccessToast"] = "Đăng ký thành công! Vui lòng kiểm tra email để xác nhận tài khoản.";
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Không tìm thấy người dùng với email '{email}'.");
            }

            if (user.EmailConfirmed)
            {
                TempData["InfoToast"] = "Email này đã được xác nhận trước đó. Bạn có thể đăng nhập.";
                return View("Login");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["SuccessToast"] = "Chúc mừng! Email đã được xác nhận thành công. Bạn có thể đăng nhập ngay bây giờ.";
                return View("Login");
            }

            // If we are here, confirmation failed (likely expired link)
            TempData["ErrorToast"] = "Liên kết xác nhận đã hết hạn hoặc không hợp lệ.";
            ViewBag.UserEmail = email; // For resend button
            return View("ConfirmEmailFailed");
        }

        [HttpGet]
        public async Task<IActionResult> ResendConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && !user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { email = user.Email, token = token }, Request.Scheme) ?? "";
                
                var subject = "Kích hoạt lại tài khoản - Finding Jobs";
                var message = $"Chào bạn {user.LastName} {user.FirstName},<br/><br/>" +
                              $"Bạn đã yêu cầu gửi lại liên kết kích hoạt tài khoản.<br/>" +
                              $"Vui lòng <a href='{confirmationLink}'>nhấn vào đây</a> để xác nhận.<br/><br/>" +
                              $"Lưu ý: Liên kết này sẽ hết hạn trong vòng <b>5 phút</b>.<br/><br/>" +
                              $"Trân trọng,<br/>Finding Jobs Team";

                if (string.IsNullOrEmpty(user.Email))
                {
                    TempData["ErrorToast"] = "Người dùng không có email hợp lệ.";
                    return View("Login");
                }

                await _authService.SendRawEmailAsync(user.Email, subject, message);
                TempData["SuccessToast"] = "Một liên kết kích hoạt mới đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư.";
            }
            else if (user != null && user.EmailConfirmed)
            {
                TempData["InfoToast"] = "Tài khoản của bạn đã được xác nhận từ trước. Bạn có thể đăng nhập ngay.";
                return View("Login");
            }

            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
             if (!ModelState.IsValid) return View(loginDto);

             // Check if user exists first
             var user = await _userManager.FindByEmailAsync(loginDto.Email);
             if (user != null && !user.EmailConfirmed)
             {
                 ViewBag.UnconfirmedEmail = user.Email;
                 TempData["ErrorToast"] = "Email chưa được xác nhận. Vui lòng kiểm tra hộp thư và xác nhận email của bạn.";
                 ModelState.AddModelError("", "Email chưa được xác nhận. Vui lòng xác nhận email trước khi đăng nhập.");
                 return View(loginDto);
             }

             var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
             
             if (result.Succeeded)
             {
                 if (user != null)
                 {
                     var roles = await _userManager.GetRolesAsync(user);
                     
                     // Store user info in session
                     HttpContext.Session.SetString("UserId", user.Id);
                     HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                     HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                     
                     if (roles.Count > 0)
                     {
                         HttpContext.Session.SetString("UserRole", roles[0]);
                         
                         // Redirect based on role
                         if (roles.Contains("Admin"))
                         {
                             TempData["SuccessToast"] = "Đăng nhập thành công! Chào mừng Admin.";
                             return RedirectToAction("Index", "Admin");
                         }
                         else if (roles.Contains("Recruiter"))
                         {
                             TempData["SuccessToast"] = "Đăng nhập thành công! Chào mừng Nhà tuyển dụng.";
                             return RedirectToAction("RecruiterDashboard", "Recruiter");
                         }
                         else if (roles.Contains("Candidate"))
                         {
                             TempData["SuccessToast"] = "Đăng nhập thành công! Chào mừng Ứng viên.";
                             return RedirectToAction("CandidateDashboard", "Candidates");
                         }
                     }
                 }
                 
                 TempData["SuccessToast"] = "Đăng nhập thành công!";
                 return RedirectToAction("Index", "Home");
             }

             TempData["ErrorToast"] = "Email hoặc mật khẩu không đúng.";
             ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
             return View(loginDto);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorToast"] = "Vui lòng nhập email.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // For security reasons, don't reveal if user exists or not
                TempData["InfoToast"] = "Nếu email này tồn tại trong hệ thống, mật khẩu mới đã được gửi đi.";
                return RedirectToAction("Login");
            }

            // [VALIDATION] Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                TempData["ErrorToast"] = "Tài khoản của bạn chưa được kích hoạt. Vui lòng kiểm tra email để kích hoạt trước khi đặt lại mật khẩu.";
                return View();
            }

            // Generate a new random password (6 alphanumeric characters)
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string newPassword = new string(Enumerable.Repeat(allowedChars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Reset password
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                TempData["ErrorToast"] = "Không thể đặt lại mật khẩu. Vui lòng thử lại sau.";
                return View();
            }

            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addResult.Succeeded)
            {
                TempData["ErrorToast"] = "Không thể thêm mật khẩu mới. Vui lòng thử lại sau.";
                return View();
            }

            // Send email
            var subject = "Mật khẩu mới của bạn - Finding Jobs";
            var message = $"Chào bạn {user.LastName} {user.FirstName},<br/><br/>" +
                          $"Mật khẩu của bạn đã được đặt lại theo yêu cầu.<br/>" +
                          $"Mật khẩu mới của bạn là: <b>{newPassword}</b><br/><br/>" +
                          $"Vui lòng đăng nhập và đổi mật khẩu ngay để đảm bảo an toàn.<br/><br/>" +
                          $"Trân trọng,<br/>Finding Jobs Team";

            await _authService.SendRawEmailAsync(user.Email!, subject, message);

            TempData["SuccessToast"] = "Mật khẩu mới đã được gửi đến email của bạn.";
            return RedirectToAction("Login");
        }
    }
}

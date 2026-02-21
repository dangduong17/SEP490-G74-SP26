
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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return View(registerDto);

            var result = await _authService.RegisterAsync(registerDto);
            if (!result)
            {
                ModelState.AddModelError("", "Registration failed. User might already exist.");
                return View(registerDto);
            }

            // Optional: Auto sign-in after register
            // await _signInManager.PasswordSignInAsync(registerDto.Email, registerDto.Password, false, false);
            
            return RedirectToAction("Login");
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

             var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
             
             if (result.Succeeded)
             {
                 var user = await _userManager.FindByEmailAsync(loginDto.Email);
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
                             return RedirectToAction("Index", "Admin");
                         }
                         else if (roles.Contains("Recruiter"))
                         {
                             return RedirectToAction("RecruiterDashboard", "Recruiter");
                         }
                         else if (roles.Contains("Candidate"))
                         {
                             return RedirectToAction("CandidateDashboard", "Candidates");
                         }
                     }
                 }
                 
                 return RedirectToAction("Index", "Home");
             }

             ModelState.AddModelError("", "Invalid login attempt.");
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
    }
}

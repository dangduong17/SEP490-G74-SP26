using Microsoft.AspNetCore.Identity;
using vn.edu.fpt.dto;
using vn.edu.fpt.entity;
using vn.edu.fpt.repository;
using vn.edu.fpt.service.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace vn.edu.fpt.service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            // Login implementation placeholder - usually involves SignInManager or verifying password
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;

            var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!checkPassword) return null;

            // Generate Token (Simplified for now)
            return "dummy-token"; 
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto, string confirmationLink)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null) 
            {
                return (false, "Email này đã được đăng ký. Vui lòng sử dụng email khác hoặc đăng nhập.");
            }

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) 
            {
                return (false, "Đăng ký thất bại. Vui lòng thử lại.");
            }

            // Ensure Role Exists
            if (!await _roleManager.RoleExistsAsync(registerDto.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);

            // Create Profile based on Role
            switch (registerDto.Role.ToLower())
            {
                case "candidate":
                    var candidate = new Candidate
                    {
                        UserId = user.Id,
                        // Initialize empty profile fields if needed
                    };
                    await _unitOfWork.Candidates.AddAsync(candidate);
                    break;

                case "recruiter":
                    var recruiter = new Recruiter
                    {
                        UserId = user.Id,
                        // Initialize empty profile fields if needed
                    };
                    await _unitOfWork.Recruiters.AddAsync(recruiter);
                    break;

                case "admin":
                    var admin = new Admin
                    {
                        UserId = user.Id,
                        // Initialize empty profile fields if needed
                    };
                    await _unitOfWork.Admins.AddAsync(admin);
                    break;

                case "user":
                    // Base user, no additional profile entity needed
                    break;
            }

            // Generate Email Confirmation Token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var fullConfirmationLink = $"{confirmationLink}&token={Uri.EscapeDataString(token)}";

            // Send Confirmation Email
            var subject = "Confirm your email - Finding Jobs";
            var message = $"Chào bạn {registerDto.LastName} {registerDto.FirstName},<br/><br/>" +
                          $"Cảm ơn bạn đã đăng ký tài khoản tại Finding Jobs.<br/>" +
                          $"Vui lòng kích hoạt tài khoản của bạn bằng cách <a href='{fullConfirmationLink}'>nhấn vào đây</a>.<br/><br/>" +
                          $"Lưu ý: Liên kết này sẽ hết hạn trong vòng <b>5 phút</b>.<br/><br/>" +
                          $"Trân trọng,<br/>Finding Jobs Team";
            await _emailService.SendEmailAsync(user.Email, subject, message);
            
            await _unitOfWork.CompleteAsync();
            return (true, null);
        }

        public async Task SendRawEmailAsync(string email, string subject, string message)
        {
            await _emailService.SendEmailAsync(email, subject, message);
        }
    }
}

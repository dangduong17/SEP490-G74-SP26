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

        public AuthService(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null) return false;

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return false;

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
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

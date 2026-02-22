using vn.edu.fpt.dto;

namespace vn.edu.fpt.service.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto, string confirmationLink);
        Task<(bool Success, string? ErrorMessage)> RegisterRecruiterAsync(RecruiterRegisterViewModel model, string confirmationLink);
        Task<string?> LoginAsync(LoginDto loginDto); // Returns JWT token or null
        Task SendRawEmailAsync(string email, string subject, string message);
    }
}

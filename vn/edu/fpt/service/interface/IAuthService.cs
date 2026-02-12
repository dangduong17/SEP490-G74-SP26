using vn.edu.fpt.dto;

namespace vn.edu.fpt.service.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<string?> LoginAsync(LoginDto loginDto); // Returns JWT token or null
    }
}

using vn.edu.fpt.dto;
using vn.edu.fpt.entity;

namespace vn.edu.fpt.service
{
    public interface IAuthService
    {
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}

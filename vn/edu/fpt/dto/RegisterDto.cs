using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập họ.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public string Role { get; set; } = null!; // "Candidate", "Recruiter", "Admin"
    }
}

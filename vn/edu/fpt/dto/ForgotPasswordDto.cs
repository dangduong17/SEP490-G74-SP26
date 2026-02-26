using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.dto
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;
    }
}

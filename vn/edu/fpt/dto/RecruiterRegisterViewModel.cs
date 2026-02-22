using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.dto
{
    public class RecruiterRegisterViewModel
    {
        // --- Tài khoản ---
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // --- Thông tin cá nhân ---
        [Required(ErrorMessage = "Họ là bắt buộc.")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vị trí công việc là bắt buộc.")]
        [MaxLength(100)]
        public string? Position { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        // --- Thông tin công ty ---
        [Required(ErrorMessage = "Tên công ty là bắt buộc.")]
        [MaxLength(255)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CompanyTaxCode { get; set; }

        [MaxLength(50)]
        public string? CompanySize { get; set; }

        [MaxLength(200)]
        public string? CompanyIndustry { get; set; }

        [MaxLength(500)]
        public string? CompanyWebsite { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? CompanyEmail { get; set; }

        [MaxLength(20)]
        public string? CompanyPhone { get; set; }

        public string? CompanyDescription { get; set; }

        public bool AgreeTerms { get; set; }
    }
}

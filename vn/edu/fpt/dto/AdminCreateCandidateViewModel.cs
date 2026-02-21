using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.dto
{
    public class AdminCreateCandidateViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ là bắt buộc.")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        public string? PhoneNumber { get; set; }

        [MaxLength(1000)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "Giới tính là bắt buộc.")]
        public string? Gender { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? District { get; set; }

        [MaxLength(100)]
        public string? CurrentPosition { get; set; }

        public int? YearsOfExperience { get; set; }
    }
}

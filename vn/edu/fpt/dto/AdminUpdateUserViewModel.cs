using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.dto
{
    public class AdminUpdateUserViewModel
    {
        [Required(ErrorMessage = "Id là bắt buộc.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ là bắt buộc.")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc.")]
        public string Role { get; set; } = "Candidate";

        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string? AdminDepartment { get; set; }

        [MaxLength(1000)]
        public string? CandidateTitle { get; set; }

        [MaxLength(100)]
        public string? CandidateCity { get; set; }

        public DateTime? CandidateDateOfBirth { get; set; }

        [MaxLength(10)]
        public string? CandidateGender { get; set; }

        [MaxLength(100)]
        public string? RecruiterPosition { get; set; }

        [MaxLength(100)]
        public string? RecruiterDepartment { get; set; }
    }
}

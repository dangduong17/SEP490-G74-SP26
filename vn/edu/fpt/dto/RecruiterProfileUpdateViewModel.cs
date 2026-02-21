using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.dto
{
    public class RecruiterProfileUpdateViewModel
    {
        [Required(ErrorMessage = "Ho la bat buoc.")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ten la bat buoc.")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "So dien thoai la bat buoc.")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vi tri cong viec la bat buoc.")]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Ten cong ty la bat buoc.")]
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

        [MaxLength(100)]
        [EmailAddress]
        public string? CompanyEmail { get; set; }

        [MaxLength(20)]
        public string? CompanyPhone { get; set; }

        public string? CompanyDescription { get; set; }

        [Required(ErrorMessage = "Địa chỉ làm việc là bắt buộc.")]
        [MaxLength(500)]
        public string WorkAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc.")]
        public int? ProvinceCode { get; set; }

        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc.")]
        [MaxLength(100)]
        public string ProvinceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phường/Xã là bắt buộc.")]
        public int? WardCode { get; set; }

        [Required(ErrorMessage = "Phường/Xã là bắt buộc.")]
        [MaxLength(100)]
        public string WardName { get; set; } = string.Empty;
    }
}

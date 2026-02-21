using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        
        [MaxLength(500)]
        public string? Logo { get; set; }
        
        [MaxLength(500)]
        public string? CoverImage { get; set; }
        
        [MaxLength(100)]
        public string? TaxCode { get; set; }
        
        [MaxLength(50)]
        public string? CompanySize { get; set; }
        
        [MaxLength(200)]
        public string? Industry { get; set; }
        
        [MaxLength(500)]
        public string? Website { get; set; }
        
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        
        [Column(TypeName = "text")]
        public string? Benefits { get; set; }
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerifiedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<CompanyAddress> Addresses { get; set; } = new List<CompanyAddress>();
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
        public ICollection<Recruiter> Recruiters { get; set; } = new List<Recruiter>();
        public ICollection<FollowedCompany> Followers { get; set; } = new List<FollowedCompany>();
        public ICollection<CompanyImage> Images { get; set; } = new List<CompanyImage>();
    }
}


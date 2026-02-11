using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Recruiter
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        
        public int? CompanyId { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
        
        [MaxLength(255)]
        public string? FullName { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(100)]
        public string? Position { get; set; }
        
        [MaxLength(100)]
        public string? Department { get; set; }
        
        [MaxLength(500)]
        public string? Avatar { get; set; }
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerifiedAt { get; set; }
        
        [MaxLength(500)]
        public string? VerificationDocument { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}

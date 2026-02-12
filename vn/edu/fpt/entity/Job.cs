using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Job : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Requirements { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public int? JobCategoryId { get; set; }
        [ForeignKey("JobCategoryId")]
        public virtual JobCategory? JobCategory { get; set; }

        public int? LocationId { get; set; } // Reference to City/Province
        [ForeignKey("LocationId")]
        public virtual Location? JobLocation { get; set; }

        [Required]
        [StringLength(50)]
        public string JobType { get; set; } = string.Empty; // Full-time, Part-time, Contract

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryMax { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Active;

        [Required]
        public int RecruiterId { get; set; }

        [ForeignKey("RecruiterId")]
        public virtual User? Recruiter { get; set; }

        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        // Navigation properties
        public virtual ICollection<Application>? Applications { get; set; }
        public virtual ICollection<SavedJob>? SavedJobs { get; set; }
        public virtual ICollection<Skill>? Skills { get; set; }
    }

    public enum JobStatus
    {
        Active = 1,
        Closed = 2,
        Draft = 3,
        Expired = 4,
        PendingApproval = 5
    }
}


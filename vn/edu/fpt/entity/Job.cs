using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        [StringLength(50)]
        public string JobType { get; set; } = string.Empty; // Full-time, Part-time, Contract

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryMax { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public DateTime? ExpiryDate { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Active;

        [Required]
        public int RecruiterId { get; set; }

        // Navigation properties
        [ForeignKey("RecruiterId")]
        public virtual User Recruiter { get; set; } = null!;

        public virtual ICollection<Application> Applications { get; set; } =
            new List<Application>();
    }

    public enum JobStatus
    {
        Active = 1,
        Closed = 2,
        Draft = 3,
        Expired = 4,
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = null!;
        
        [Required]
        public int CompanyId { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;
        
        [Required]
        public int RecruiterId { get; set; }
        
        [ForeignKey(nameof(RecruiterId))]
        public Recruiter Recruiter { get; set; } = null!;
        
        public int? CompanyAddressId { get; set; }
        
        [ForeignKey(nameof(CompanyAddressId))]
        public CompanyAddress? CompanyAddress { get; set; }
        
        [Required]
        [Column(TypeName = "text")]
        public string Description { get; set; } = null!;
        
        [Column(TypeName = "text")]
        public string? Requirements { get; set; }
        
        [Column(TypeName = "text")]
        public string? Benefits { get; set; }
        
        [MaxLength(100)]
        public string? Level { get; set; }
        
        [MaxLength(100)]
        public string? JobType { get; set; }
        
        [MaxLength(100)]
        public string? WorkingType { get; set; }
        
        public decimal? MinSalary { get; set; }
        
        public decimal? MaxSalary { get; set; }
        
        public bool IsNegotiable { get; set; } = false;
        
        [MaxLength(50)]
        public string? SalaryCurrency { get; set; } = "VND";
        
        public int NumberOfPositions { get; set; } = 1;
        
        [MaxLength(100)]
        public string? Gender { get; set; }
        
        public int? MinAge { get; set; }
        
        public int? MaxAge { get; set; }
        
        public int? MinYearsOfExperience { get; set; }
        
        [MaxLength(100)]
        public string? DegreeRequired { get; set; }
        
        public DateTime? ApplicationDeadline { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Draft";
        
        public int ViewCount { get; set; } = 0;
        
        public int ApplicationCount { get; set; } = 0;
        
        public bool IsFeatured { get; set; } = false;
        
        public bool IsUrgent { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        
        // Navigation properties
        public ICollection<JobSkill> Skills { get; set; } = new List<JobSkill>();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<SavedJob> SavedByUsers { get; set; } = new List<SavedJob>();
    }
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        
        [MaxLength(255)]
        public string? FullName { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [MaxLength(10)]
        public string? Gender { get; set; }
        
        [MaxLength(500)]
        public string? Address { get; set; }
        
        [MaxLength(100)]
        public string? City { get; set; }
        
        [MaxLength(100)]
        public string? District { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(500)]
        public string? Avatar { get; set; }
        
        [MaxLength(1000)]
        public string? Title { get; set; }
        
        public decimal? CurrentSalary { get; set; }
        
        public decimal? ExpectedSalary { get; set; }
        
        [MaxLength(50)]
        public string? WorkingType { get; set; }
        
        [Column(TypeName = "text")]
        public string? Summary { get; set; }
        
        [MaxLength(100)]
        public string? CurrentPosition { get; set; }
        
        public int? YearsOfExperience { get; set; }
        
        [MaxLength(100)]
        public string? HighestDegree { get; set; }
        
        public bool IsLookingForJob { get; set; } = true;
        
        public bool AllowContact { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<CV> CVs { get; set; } = new List<CV>();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
        public ICollection<FollowedCompany> FollowedCompanies { get; set; } = new List<FollowedCompany>();
        public ICollection<CandidateSkill> Skills { get; set; } = new List<CandidateSkill>();
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
    }
}


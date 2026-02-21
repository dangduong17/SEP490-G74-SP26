using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.entity
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [MaxLength(100)]
        public string? Category { get; set; }
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        // Navigation properties
        public ICollection<JobSkill> Jobs { get; set; } = new List<JobSkill>();
        public ICollection<CandidateSkill> Candidates { get; set; } = new List<CandidateSkill>();
    }
}


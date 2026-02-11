using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string Position { get; set; } = null!;
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentlyWorking { get; set; } = false;
        
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

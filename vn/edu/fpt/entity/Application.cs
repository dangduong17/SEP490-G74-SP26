using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Application
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int JobId { get; set; }
        
        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;
        
        [Required]
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        [Required]
        public int CVId { get; set; }
        
        [ForeignKey(nameof(CVId))]
        public CV CV { get; set; } = null!;
        
        [Column(TypeName = "text")]
        public string? CoverLetter { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";
        
        public DateTime? ReviewedAt { get; set; }
        
        public int? ReviewedBy { get; set; }
        
        [Column(TypeName = "text")]
        public string? ReviewNotes { get; set; }
        
        public int? Rating { get; set; }
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        public DateTime? UpdatedAt { get; set; }
    }
}



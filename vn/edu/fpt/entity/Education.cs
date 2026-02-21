using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string School { get; set; } = null!;
        
        [MaxLength(255)]
        public string? Major { get; set; }
        
        [MaxLength(100)]
        public string? Degree { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentlyStudying { get; set; } = false;
        
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        
        public decimal? GPA { get; set; }
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
    }
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class CV
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CandidateId { get; set; }
        
        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = null!;
        
        [MaxLength(500)]
        public string? FilePath { get; set; }
        
        [MaxLength(100)]
        public string? TemplateId { get; set; }
        
        [Column(TypeName = "text")]
        public string? JsonData { get; set; }
        
        public bool IsDefault { get; set; } = false;
        
        public int ViewCount { get; set; } = 0;
        
        public int DownloadCount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}



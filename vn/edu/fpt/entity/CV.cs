using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class CV : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Summary { get; set; }

        public string? Experience { get; set; }

        public string? Education { get; set; }

        public string? Certifications { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; } // Path to uploaded CV file

        [StringLength(50)]
        public string? FileType { get; set; } // PDF, DOC, etc.

        [Required]
        public int CandidateId { get; set; }

        public bool IsDefault { get; set; } = false;

        // Navigation properties
        [ForeignKey("CandidateId")]
        public virtual User? Candidate { get; set; }

        public virtual ICollection<Application>? Applications { get; set; }
        public virtual ICollection<Skill>? Skills { get; set; }
    }
}


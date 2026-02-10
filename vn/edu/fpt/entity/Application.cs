using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Application : BaseEntity
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        public int? CVId { get; set; }

        public string? CoverLetter { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        public string? RecruiterNotes { get; set; }

        public DateTime? InterviewDate { get; set; }

        public string? InterviewNotes { get; set; }

        // Navigation properties
        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        [ForeignKey("CandidateId")]
        public virtual User? Candidate { get; set; }

        [ForeignKey("CVId")]
        public virtual CV? CV { get; set; }
    }

    public enum ApplicationStatus
    {
        Submitted = 1,
        UnderReview = 2,
        Shortlisted = 3,
        InterviewScheduled = 4,
        Interviewed = 5,
        Offered = 6,
        Rejected = 7,
        Withdrawn = 8,
        Hired = 9,
    }
}


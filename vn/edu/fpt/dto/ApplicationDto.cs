namespace vn.edu.fpt.dto
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string? JobTitle { get; set; }
        public string? CandidateName { get; set; }
        public string? CVTitle { get; set; }
        public string? CoverLetter { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RecruiterNotes { get; set; }
        public DateTime? InterviewDate { get; set; }
    }
}

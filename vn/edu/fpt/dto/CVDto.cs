namespace vn.edu.fpt.dto
{
    public class CVDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Experience { get; set; }
        public string? Education { get; set; }
        public string? Certifications { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public bool IsDefault { get; set; }
        public string? CandidateName { get; set; }
    }
}

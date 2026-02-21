namespace vn.edu.fpt.dto
{
    public class RecruiterDashboardViewModel
    {
        public string RecruiterName { get; set; } = string.Empty;
        public string? RecruiterEmail { get; set; }
        public string? RecruiterPhone { get; set; }
        public string? Position { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyIndustry { get; set; }
        public bool IsVerified { get; set; }
    }
}

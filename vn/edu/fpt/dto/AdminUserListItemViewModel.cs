namespace vn.edu.fpt.dto
{
    public class AdminUserListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "N/A";
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}

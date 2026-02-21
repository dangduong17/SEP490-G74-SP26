using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.entity
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        
        [MaxLength(100)]
        public string? LastName { get; set; }
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public Candidate? Candidate { get; set; }
        public Recruiter? Recruiter { get; set; }
        public Admin? Admin { get; set; }
    }
}


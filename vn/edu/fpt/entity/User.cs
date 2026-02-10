using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class User : IdentityUser<int>
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<Job>? Jobs { get; set; } // For recruiters
        public virtual ICollection<Application>? Applications { get; set; } // For candidates
        public virtual ICollection<CV>? CVs { get; set; } // For candidates
        public virtual ICollection<SavedJob>? SavedJobs { get; set; }
        public virtual ICollection<Subscription>? Subscriptions { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }

    public enum UserRole
    {
        Admin = 1,
        Recruiter = 2,
        Candidate = 3,
    }
}

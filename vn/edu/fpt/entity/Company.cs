using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Company : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? LogoUrl { get; set; }

        public string? Website { get; set; }

        public string? Address { get; set; }

        public string? ContactEmail { get; set; }

        public string? ContactPhone { get; set; }

        // Navigation properties
        public virtual ICollection<Job>? Jobs { get; set; }
        public virtual ICollection<User>? Recruiters { get; set; } // Recruiters belonging to this company
    }
}

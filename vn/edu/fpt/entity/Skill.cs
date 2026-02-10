using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.entity
{
    public class Skill : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Job>? Jobs { get; set; }
        public virtual ICollection<CV>? CVs { get; set; }
    }
}

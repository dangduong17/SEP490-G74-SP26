using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.entity
{
    public class JobCategory : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}

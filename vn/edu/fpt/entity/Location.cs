using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.entity
{
    public class Location : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CityName { get; set; } = string.Empty;

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}

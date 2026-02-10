using System.ComponentModel.DataAnnotations;

namespace vn.edu.fpt.entity
{
    public class SubscriptionPlan : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        public int MaxJobPosts { get; set; } // Example limit

        // Navigation properties
        public virtual ICollection<Subscription>? Subscriptions { get; set; }
    }
}

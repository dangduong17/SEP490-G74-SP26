using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class SubscriptionPlan
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        
        public int DurationDays { get; set; }
        
        public string? Description { get; set; }
        
        // Target audience for the plan
        public PlanTarget TargetAudience { get; set; } = PlanTarget.Both;
        
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Subscription>? Subscriptions { get; set; }
    }

    public enum PlanTarget
    {
        Candidate = 1,
        Recruiter = 2,
        Both = 3
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        
        public int PlanId { get; set; }
        
        [ForeignKey(nameof(PlanId))]
        public SubscriptionPlan Plan { get; set; } = null!;
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Active";
        
        public DateTime CreatedAt { get; set; } = vn.edu.fpt.helper.DateTimeHelper.NowVietnam;
    }
}


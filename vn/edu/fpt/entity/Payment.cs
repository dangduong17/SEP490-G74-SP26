using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        
        public int SubscriptionId { get; set; }
        
        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; } = null!;
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        
        [MaxLength(100)]
        public string? TransactionId { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";
        
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }
    }
}

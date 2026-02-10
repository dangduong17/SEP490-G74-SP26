using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vn.edu.fpt.entity
{
    public class Payment : BaseEntity
    {
        [Required]
        public int SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public virtual Subscription? Subscription { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string? TransactionId { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? PaymentMethod { get; set; } // e.g., VNPay, Momo, Credit Card
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Success = 2,
        Failed = 3
    }
}

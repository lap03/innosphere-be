using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Payment : BaseEntity
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Subscription ID is required.")]
        [ForeignKey("Subscription")]
        public int? SubscriptionId { get; set; }

        [Required(ErrorMessage = "Payment type ID is required.")]
        [ForeignKey("PaymentType")]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "Advertisement ID is required.")]
        [ForeignKey("Advertisement")]
        public int? AdvertisementId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        [StringLength(100, ErrorMessage = "Transaction ID cannot exceed 100 characters.")]
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [StringLength(20)]
        [RegularExpression("^(PAID|PENDING|FAILED)$", ErrorMessage = "Invalid payment status.")]
        public string PaymentStatus { get; set; } = "PENDING";

        public string PaymentDetails { get; set; }

        public virtual Employer Employer { get; set; }
        public virtual Subscription Subscription { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}

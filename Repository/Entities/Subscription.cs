using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Subscription : BaseEntity
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Subscription package ID is required.")]
        [ForeignKey("SubscriptionPackage")]
        public int SubscriptionPackageId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Amount paid is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public float AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [StringLength(20)]
        [RegularExpression("^(PAID|PENDING|FAILED)$", ErrorMessage = "Invalid payment status.")]
        public string PaymentStatus { get; set; } = "PENDING";

        public virtual Employer Employer { get; set; }
        public virtual SubscriptionPackage SubscriptionPackage { get; set; }

        public virtual ICollection<JobPosting> JobPostings { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

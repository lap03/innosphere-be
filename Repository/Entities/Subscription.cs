using Repository.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    public decimal AmountPaid { get; set; }  

    [Required(ErrorMessage = "Payment status is required.")]
    [StringLength(20)]
    [RegularExpression("^(PAID|PENDING|FAILED)$", ErrorMessage = "Invalid payment status.")]
    public string PaymentStatus { get; set; } = "PENDING";

    [StringLength(100, ErrorMessage = "Transaction ID cannot exceed 100 characters.")]
    public string? TransactionId { get; set; }  // nullable nếu có thể chưa có

    public virtual Employer Employer { get; set; }
    public virtual SubscriptionPackage SubscriptionPackage { get; set; }

    public virtual ICollection<JobPosting> JobPostings { get; set; }
}

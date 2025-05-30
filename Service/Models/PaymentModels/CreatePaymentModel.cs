using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PaymentModels
{
    public class CreatePaymentModel
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Subscription ID is required.")]
        public int? SubscriptionId { get; set; }

        [Required(ErrorMessage = "Payment type ID is required.")]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "Advertisement ID is required.")]
        public int? AdvertisementId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        [StringLength(100, ErrorMessage = "Transaction ID cannot exceed 100 characters.")]
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [RegularExpression("^(PAID|PENDING|FAILED)$", ErrorMessage = "Invalid payment status.")]
        public string PaymentStatus { get; set; } = "PENDING";

        public string PaymentDetails { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SubscriptionModels
{
    public class CreateSubscriptionModel
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "SubscriptionPackage ID is required.")]
        public int SubscriptionPackageId { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "AmountPaid is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "AmountPaid must be greater than 0.")]
        public float AmountPaid { get; set; }

        [Required(ErrorMessage = "PaymentStatus is required.")]
        [StringLength(20)]
        [RegularExpression("^(PAID)$", ErrorMessage = "Invalid PaymentStatus platform.")]
        public string PaymentStatus { get; set; } = "PAID";

        [StringLength(100)]
        public string TransactionId { get; set; }
    }
}

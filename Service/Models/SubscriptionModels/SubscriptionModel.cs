using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SubscriptionModels
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public int SubscriptionPackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public float AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
    }
}

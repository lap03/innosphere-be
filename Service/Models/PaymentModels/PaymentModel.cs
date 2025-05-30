using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PaymentModels
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public int? SubscriptionId { get; set; }
        public int PaymentTypeId { get; set; }
        public int? AdvertisementId { get; set; }
        public float Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentDetails { get; set; }
    }
}

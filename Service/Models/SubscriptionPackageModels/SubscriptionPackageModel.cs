using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SubscriptionPackageModels
{
    public class SubscriptionPackageModel
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int DurationDays { get; set; }
        public int JobPostLimit { get; set; }
        public bool AllowUrgentPosts { get; set; }
        public bool AllowHighlightedPosts { get; set; }
        public int? ProfileViewsLimit { get; set; }
        public int? ProfileStorageDays { get; set; }
        public bool AllowBrandPromotion { get; set; }
        public bool IsActive { get; set; }
    }
}

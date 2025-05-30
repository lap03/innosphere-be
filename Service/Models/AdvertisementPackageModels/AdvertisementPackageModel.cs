using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.AdvertisementPackageModels
{
    public class AdvertisementPackageModel
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int DurationDays { get; set; }
        public int? MaxImpressions { get; set; }
        public string AdPosition { get; set; }
        public string AllowedAdTypes { get; set; }
        public bool IsActive { get; set; }
    }
}

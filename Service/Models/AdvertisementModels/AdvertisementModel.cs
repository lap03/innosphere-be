using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.AdvertisementModels
{
    public class AdvertisementModel
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public int AdvertisementPackageId { get; set; }
        public string AdTitle { get; set; }
        public string AdDescription { get; set; }
        public string ImageUrl { get; set; }
        public string AdPosition { get; set; }
        public string AdStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Price { get; set; }
        public int? MaxImpressions { get; set; }
        public int CurrentImpressions { get; set; }
        public string TransactionId { get; set; }

        // Related data for admin display
        public string? EmployerUserName { get; set; }
        public string? EmployerFullName { get; set; }
    }
}

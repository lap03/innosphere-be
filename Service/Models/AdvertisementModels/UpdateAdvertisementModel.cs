using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.AdvertisementModels
{
    public class UpdateAdvertisementModel
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Advertisement package ID is required.")]
        public int AdvertisementPackageId { get; set; }

        [Required(ErrorMessage = "Ad title is required.")]
        [StringLength(255, ErrorMessage = "Ad title cannot exceed 255 characters.")]
        public string AdTitle { get; set; }

        public string AdDescription { get; set; }

        [StringLength(512, ErrorMessage = "Image URL cannot exceed 512 characters.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Ad position is required.")]
        [RegularExpression("^(Top|Sidebar|DetailPage)$", ErrorMessage = "Position must be Top, Sidebar, or DetailPage.")]
        public string AdPosition { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public float Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Max impressions must be positive.")]
        public int? MaxImpressions { get; set; }

        [StringLength(100, ErrorMessage = "Transaction ID cannot exceed 100 characters.")]
        public string TransactionId { get; set; }
    }
}

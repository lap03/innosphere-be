using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Advertisement : BaseEntity
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Advertisement package ID is required.")]
        [ForeignKey("AdvertisementPackage")]
        public int AdvertisementPackageId { get; set; }

        [Required(ErrorMessage = "Ad title is required.")]
        [StringLength(255, ErrorMessage = "Ad title cannot exceed 255 characters.")]
        public string AdTitle { get; set; }

        public string AdDescription { get; set; }

        [StringLength(512, ErrorMessage = "Image URL cannot exceed 512 characters.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Ad position is required.")]
        [StringLength(50)]
        [RegularExpression("^(Top|Sidebar|Footer)$", ErrorMessage = "Position must be Top, Sidebar, or Footer.")]
        public string AdPosition { get; set; }

        [StringLength(20)]
        [RegularExpression("^(PENDING|ACTIVE|INACTIVE|EXPIRED)$", ErrorMessage = "Invalid ad status.")]
        public string AdStatus { get; set; } = "PENDING";

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public float Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Max impressions must be positive.")]
        public int? MaxImpressions { get; set; }

        public int CurrentImpressions { get; set; } = 0;

        public virtual Employer Employer { get; set; }
        public virtual AdvertisementPackage AdvertisementPackage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SubscriptionPackageModels
{
    public class UpdateSubscriptionPackageModel
    {
        [Required(ErrorMessage = "Package name is required.")]
        [StringLength(100, ErrorMessage = "Package name cannot exceed 100 characters.")]
        public string PackageName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Duration (in days) is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 day.")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Job post limit is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Job post limit must be zero or positive.")]
        public int JobPostLimit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Profile views limit must be positive.")]
        public int? ProfileViewsLimit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Profile storage days must be positive.")]
        public int? ProfileStorageDays { get; set; }

        public bool AllowUrgentPosts { get; set; } = false;
        public bool AllowHighlightedPosts { get; set; } = false;
        public bool AllowBrandPromotion { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}

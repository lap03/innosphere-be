using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Employer : BaseEntity
    {
        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(255, ErrorMessage = "Company name cannot exceed 255 characters.")]
        public string CompanyName { get; set; }

        [Required]
        [ForeignKey("BusinessType")]
        public int BusinessTypeId { get; set; }

        [StringLength(100, ErrorMessage = "Company size cannot exceed 100 characters.")]
        public string? CompanySize { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfIncorporation { get; set; }
        [StringLength(512, ErrorMessage = "Company logo URL cannot exceed 512 characters.")]
        public string? CompanyLogoUrl { get; set; }

        [StringLength(500, ErrorMessage = "Company website URL cannot exceed 500 characters.")]
        public string? CompanyWebsite { get; set; }

        [StringLength(500, ErrorMessage = "Company cover URL cannot exceed 500 characters.")]
        public string? CompanyCoverUrl { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string? CompanyAddress { get; set; }

        [StringLength(50, ErrorMessage = "Tax code cannot exceed 50 characters.")]
        public string? TaxCode { get; set; }

        public string? CompanyDescription { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float Rating { get; set; } = 0;

        public int TotalRatings { get; set; } = 0;
        public bool IsVerified { get; set; } = false;

        public virtual User User { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public virtual ICollection<JobPosting> JobPostings { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<EmployerRating> EmployerRatings { get; set; }
    }
}

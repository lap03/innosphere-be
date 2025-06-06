using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class JobPosting : BaseEntity
    {
        [Required(ErrorMessage = "Employer ID is required.")]
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }

        [Required(ErrorMessage = "Subscription ID is required.")]
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        [ForeignKey("City")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(255, ErrorMessage = "Job title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Job description is required.")]
        [StringLength(500, ErrorMessage = "Job cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters.")]
        public string Location { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be positive.")]
        public float? HourlyRate { get; set; }

        [Required(ErrorMessage = "Job type is required.")]
        [StringLength(20)]
        [RegularExpression("^(FullTime|PartTime|Freelance)$", ErrorMessage = "Invalid job type.")]
        public string JobType { get; set; }

        public string? Requirements { get; set; }

        [Required(ErrorMessage = "Posted date is required.")]
        public DateTime PostedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        [StringLength(20)]
        [RegularExpression("^(PENDING|APPROVED|CLOSED|REJECT)$", ErrorMessage = "Invalid job status.")]
        public string Status { get; set; } = "PENDING";

        public bool IsUrgent { get; set; } = false;
        public bool IsHighlighted { get; set; } = false;

        public int ViewsCount { get; set; } = 0;
        public int ApplicationsCount { get; set; } = 0;

        public virtual Employer Employer { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }
        public virtual ICollection<JobPostingTag> JobPostingTags { get; set; }
        public virtual ICollection<EmployerRating> EmployerRatings { get; set; }
        public virtual ICollection<WorkerRating> WorkerRatings { get; set; }
    }
}

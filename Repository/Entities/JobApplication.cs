using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class JobApplication : BaseEntity
    {
        [Required(ErrorMessage = "Job ID is required.")]
        [ForeignKey("JobPosting")]
        public int JobPostingId { get; set; }

        [ForeignKey("Resume")]
        public int? ResumeId { get; set; }

        [Required(ErrorMessage = "Applied date is required.")]
        public DateTime AppliedAt { get; set; }

        [StringLength(20)]
        [RegularExpression("^(PENDING|REVIEWED|ACCEPTED|REJECTED)$", ErrorMessage = "Invalid status.")]
        public string Status { get; set; } = "PENDING";

        public string CoverNote { get; set; }
        public virtual JobPosting JobPosting { get; set; }
        public virtual Resume Resume { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual Worker Worker { get; set; }
    }
}

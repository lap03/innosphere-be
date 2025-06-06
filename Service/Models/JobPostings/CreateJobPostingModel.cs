using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.JobPostings
{
    public class CreateJobPostingModel
    {
        [Required]
        public int EmployerId { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        public int? CityId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [Range(0, double.MaxValue)]
        public float? HourlyRate { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("^(FullTime|PartTime|Freelance)$")]
        public string JobType { get; set; }

        public string? Requirements { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsUrgent { get; set; }
        public bool IsHighlighted { get; set; }
        public List<int> TagIds { get; set; }
    }
}

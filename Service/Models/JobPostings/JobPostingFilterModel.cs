using System;
using System.ComponentModel.DataAnnotations;

namespace Service.Models.JobPostings
{
    public class JobPostingFilterModel
    {
        public int? CityId { get; set; }

        [StringLength(20)]
        [RegularExpression("^(FullTime|PartTime|Freelance)$", ErrorMessage = "Invalid job type.")]
        public string? JobType { get; set; }

        [StringLength(20)]
        [RegularExpression("^(PENDING|APPROVED|CLOSED)$", ErrorMessage = "Invalid job status.")]
        public string? Status { get; set; }

        public float? MinHourlyRate { get; set; }
        public float? MaxHourlyRate { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? EndTo { get; set; }
        public string? Keyword { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
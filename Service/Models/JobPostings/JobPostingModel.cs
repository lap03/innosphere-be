using Service.Models.JobTagModels;
using System;
using System.Collections.Generic;

namespace Service.Models.JobPostings
{

    public class JobPostingModel
    {
        public int Id { get; set; }     
        public int EmployerId { get; set; }
        public int SubscriptionId { get; set; }
        public int? CityId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public float? HourlyRate { get; set; }
        public string JobType { get; set; }
        public string? Requirements { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string Status { get; set; }
        public bool IsUrgent { get; set; }
        public bool IsHighlighted { get; set; }
        public int ViewsCount { get; set; }
        public int ApplicationsCount { get; set; }
        public List<JobTagModel> JobTags { get; set; }
    }
}
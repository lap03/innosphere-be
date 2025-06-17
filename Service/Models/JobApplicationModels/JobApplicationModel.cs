using Service.Models.JobPostings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.JobApplicationModels
{
    public class JobApplicationModel
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public JobPostingModel JobPosting { get; set; }
        public int? ResumeId { get; set; }
        public DateTime AppliedAt { get; set; }
        public string Status { get; set; }
        public string CoverNote { get; set; }

        // Các trường thông tin bổ sung có thể mở rộng nếu cần
        public string JobTitle { get; set; }
        public string WorkerName { get; set; }
        public string ResumeTitle { get; set; }

        // Worker Profile Information
        public WorkerProfileModel WorkerProfile { get; set; }
    }

    // Enhanced Worker Profile Model for JobApplication responses
    public class WorkerProfileModel
    {
        public string UserId { get; set; }
        public string Skills { get; set; }
        public string Bio { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public float Rating { get; set; }
        public int TotalRatings { get; set; }
        public string VerificationStatus { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string ContactLocation { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string PersonalWebsite { get; set; }

        // Additional User information
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
    }
}

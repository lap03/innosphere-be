using Service.Models.SocialLinkModels;
using System;

namespace Service.Models.EmployerModels
{
    public class EmployerModel
    {
        // User info
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Employer info
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public int BusinessTypeId { get; set; }
        public string? CompanyAddress { get; set; }
        public string? TaxCode { get; set; }
        public string? CompanyDescription { get; set; }
        public float Rating { get; set; }
        public int TotalRatings { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<SocialLinkModel> SocialLinks { get; set; }

    }
}   
using innosphere_be.Models.responses.BusinessTypeResponses;

namespace innosphere_be.Models.responses.EmployerResponses
{
    public class EmployerProfileResponse
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public BusinessTypeResponse BusinessType { get; set; }
        public string? CompanyAddress { get; set; }
        public string? TaxCode { get; set; }
        public string? CompanyDescription { get; set; }
        public float Rating { get; set; }
        public int TotalRatings { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

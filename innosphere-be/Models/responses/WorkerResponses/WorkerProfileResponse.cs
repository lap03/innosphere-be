namespace innosphere_be.Models.responses.WorkerResponses
{
    public class WorkerProfileResponse
    {
        public int Id { get; set; }
        public string? Skills { get; set; }
        public string? Bio { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public float Rating { get; set; }
        public int TotalRatings { get; set; }
        public string VerificationStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

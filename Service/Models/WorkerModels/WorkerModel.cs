using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.WorkerModels
{
    public class WorkerModel
    {
        // User info
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Worker info
        public int WorkerId { get; set; }
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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Worker : BaseEntity
    {
        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [StringLength(255, ErrorMessage = "Skills cannot exceed 255 characters.")]
        public string? Skills { get; set; }

        [StringLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
        public string? Bio { get; set; }

        public string? Education { get; set; }
        public string? Experience { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float Rating { get; set; } = 0;

        public int TotalRatings { get; set; } = 0;

        [Required(ErrorMessage = "Verification status is required.")]
        [StringLength(20)]
        [RegularExpression("^(PENDING|APPROVED|REJECTED)$", ErrorMessage = "Status must be PENDING, APPROVED or REJECTED.")]
        public string VerificationStatus { get; set; } = "PENDING";

        public virtual User User { get; set; }
        public virtual ICollection<Resume> Resumes { get; set; }
        public virtual ICollection<WorkerRating> WorkerRatings { get; set; }
    }
}

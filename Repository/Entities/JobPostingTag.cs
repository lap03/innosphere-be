using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class JobPostingTag : BaseEntity
    {
        [Required(ErrorMessage = "Job ID is required.")]
        [ForeignKey("JobPosting")]
        public int JobPostingId { get; set; }

        [Required(ErrorMessage = "Tag ID is required.")]
        [ForeignKey("JobTag")]
        public int JobTagId { get; set; }

        public virtual JobPosting JobPosting { get; set; }
        public virtual JobTag JobTag { get; set; }
    }
}

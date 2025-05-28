using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class EmployerRating : BaseEntity
    {
        [Required(ErrorMessage = "Application ID is required.")]
        [ForeignKey("JobApplication")]
        public int JobApplicationId { get; set; }

        [Required(ErrorMessage = "Employer ID is required.")]
        [ForeignKey("Employer")]
        public int EmployerId { get; set; }
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float RatingValue { get; set; }
        public string Comment { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.Now;

        public virtual JobApplication JobApplication { get; set; }
        public virtual Employer Employer { get; set; }

        public virtual ICollection<EmployerRatingCriteria> RatingCriterias { get; set; }
    }
}

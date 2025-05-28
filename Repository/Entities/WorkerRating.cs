using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class WorkerRating : BaseEntity
    {
        [Required(ErrorMessage = "Application ID is required.")]
        [ForeignKey("JobApplication")]
        public int JobApplicationId { get; set; }

        [Required(ErrorMessage = "Worker ID is required.")]
        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float RatingValue { get; set; }

        public string Comment { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.Now;

        public virtual JobApplication JobApplication { get; set; }
        public virtual Worker Worker { get; set; }

        public virtual ICollection<WorkerRatingCriteria> RatingCriterias { get; set; }
    }
}

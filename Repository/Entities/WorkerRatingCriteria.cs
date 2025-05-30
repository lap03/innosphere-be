using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class WorkerRatingCriteria : BaseEntity
    {
        [Required(ErrorMessage = "WorkerRating ID is required.")]
        [ForeignKey("WorkerRating")]
        public int WorkerRatingId { get; set; }

        [Required(ErrorMessage = "Criteria ID is required.")]
        [ForeignKey("RatingCriteria")]
        public int RatingCriteriaId { get; set; }

        [Required(ErrorMessage = "Score is required.")]
        [Range(0, 5, ErrorMessage = "Score must be between 0 and 5.")]
        public float Score { get; set; }

        public virtual WorkerRating WorkerRating { get; set; }
        public virtual RatingCriteria RatingCriteria { get; set; }
    }
}

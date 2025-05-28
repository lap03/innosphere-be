using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class EmployerRatingCriteria : BaseEntity
    {
        [Required(ErrorMessage = "EmployerRating ID is required.")]
        [ForeignKey("EmployerRating")]
        public int EmployerRatingId { get; set; }

        [Required(ErrorMessage = "Criteria ID is required.")]
        [ForeignKey("RatingCriteria")]
        public int RatingCriteriaId { get; set; }

        [Required(ErrorMessage = "Score is required.")]
        [Range(0, 5, ErrorMessage = "Score must be between 0 and 5.")]
        public float Score { get; set; }

        public virtual EmployerRating EmployerRating { get; set; }
        public virtual RatingCriteria RatingCriteria { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class RatingCriteria : BaseEntity
    {
        [Required(ErrorMessage = "Criteria name is required.")]
        [StringLength(100, ErrorMessage = "Criteria name cannot exceed 100 characters.")]
        public string CriteriaName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Criteria type is required.")]
        [StringLength(20, ErrorMessage = "Criteria type must be either 'Worker' or 'Employer'.")]
        public string CriteriaType { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<WorkerRatingCriteria> WorkerRatingCriterias { get; set; }
        public virtual ICollection<EmployerRatingCriteria> EmployerRatingCriterias { get; set; }
    }
}

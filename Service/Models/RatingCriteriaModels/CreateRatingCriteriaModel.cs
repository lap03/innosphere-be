using System.ComponentModel.DataAnnotations;

namespace Service.Models.RatingCriteriaModels
{
    public class CreateRatingCriteriaModel
    {
        [Required(ErrorMessage = "Criteria name is required.")]
        [StringLength(100, ErrorMessage = "Criteria name cannot exceed 100 characters.")]
        public string CriteriaName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Criteria type is required.")]
        [StringLength(20, ErrorMessage = "Criteria type must be either 'Worker' or 'Employer'.")]
        [RegularExpression("^(Worker|Employer)$", ErrorMessage = "Criteria type must be either 'Worker' or 'Employer'.")]
        public string CriteriaType { get; set; }
    }
}
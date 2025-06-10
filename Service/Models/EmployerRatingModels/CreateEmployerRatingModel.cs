using Service.Models.EmployerRatingCriteriaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.EmployerRatingModels
{
    public class CreateEmployerRatingModel
    {
        [Required]
        public int JobApplicationId { get; set; }

        [Required]
        public int EmployerId { get; set; }

        [Range(0, 5)]
        public float RatingValue { get; set; } // Sẽ tính lại ở backend

        public string? Comment { get; set; }

        [Required]
        public List<CreateEmployerRatingDetailModel> Details { get; set; }
    }
}

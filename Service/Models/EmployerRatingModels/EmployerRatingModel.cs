using Service.Models.EmployerRatingCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.EmployerRatingModels
{
    public class EmployerRatingModel
    {
        public int Id { get; set; }
        public int JobApplicationId { get; set; }
        public int EmployerId { get; set; }
        public float RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime RatedAt { get; set; }
        public List<EmployerRatingCriteriaModel> Details { get; set; }
    }
}

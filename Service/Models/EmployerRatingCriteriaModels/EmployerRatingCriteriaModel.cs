using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.EmployerRatingCriteriaModels
{
    public class EmployerRatingCriteriaModel
    {
        public int Id { get; set; }
        public int EmployerRatingId { get; set; }
        public int RatingCriteriaId { get; set; }
        public float Score { get; set; }
        public string? CriteriaName { get; set; }
        public string? CriteriaDescription { get; set; }
    }
}

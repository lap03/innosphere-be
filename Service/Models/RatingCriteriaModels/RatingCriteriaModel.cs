using System.ComponentModel.DataAnnotations;

namespace Service.Models.RatingCriteriaModels
{
    public class RatingCriteriaModel
    {
        public int Id { get; set; }
        public string CriteriaName { get; set; }
        public string Description { get; set; }
        public string CriteriaType { get; set; }
    }
}
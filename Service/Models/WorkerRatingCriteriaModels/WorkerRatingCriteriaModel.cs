using System;

namespace Service.Models.WorkerRatingCriteriaModels
{
    public class WorkerRatingCriteriaModel
    {
        public int Id { get; set; }
        public int WorkerRatingId { get; set; }
        public int RatingCriteriaId { get; set; }
        public float Score { get; set; }

        // Optional: Include info about the criteria itself for richer detail
        public string? CriteriaName { get; set; }
        public string? CriteriaDescription { get; set; }
    }
}
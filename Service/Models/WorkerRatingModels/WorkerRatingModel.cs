using Service.Models.WorkerRatingCriteriaModels;
using System;
using System.Collections.Generic;

namespace Service.Models.WorkerRatingModels
{
    public class WorkerRatingModel
    {
        public int Id { get; set; }
        public int JobApplicationId { get; set; }
        public int WorkerId { get; set; }
        public float RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime RatedAt { get; set; }

        // Danh sách chi tiết đánh giá theo từng tiêu chí
        public List<WorkerRatingCriteriaModel> Details { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.EmployerRatingCriteriaModels
{
    public class CreateEmployerRatingDetailModel
    {
        [Required]
        public int RatingCriteriaId { get; set; }

        [Range(0, 5)]
        public float Score { get; set; }
    }
}

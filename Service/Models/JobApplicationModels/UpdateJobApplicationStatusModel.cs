using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.JobApplicationModels
{
    public class UpdateJobApplicationStatusModel
    {
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(PENDING|REVIEWED|ACCEPTED|REJECTED)$", ErrorMessage = "Invalid status.")]
        public string Status { get; set; }
    }
}

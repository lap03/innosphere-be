using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.JobApplicationModels
{
    public class CreateJobApplicationModel
    {
        [Required(ErrorMessage = "JobPostingId is required.")]
        public int JobPostingId { get; set; }
        [Required(ErrorMessage = "ResumeId is required.")]
        public int? ResumeId { get; set; }

        public string CoverNote { get; set; }
    }
}

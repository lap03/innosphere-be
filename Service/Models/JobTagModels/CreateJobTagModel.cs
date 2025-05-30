using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.JobTagModels
{
    public class CreateJobTagModel
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(100, ErrorMessage = "Tag name cannot exceed 100 characters.")]
        public string TagName { get; set; }
    }
}

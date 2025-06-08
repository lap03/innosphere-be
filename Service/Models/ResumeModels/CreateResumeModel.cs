using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Service.Models.ResumeModels
{
    public class CreateResumeModel
    {
        [Required(ErrorMessage = "Worker ID is required.")]
        public int WorkerId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "UrlCvs is required.")]
        [StringLength(512, ErrorMessage = "UrlCvs cannot exceed 512 characters.")]
        public string UrlCvs { get; set; }

        [Required(ErrorMessage = "File type is required.")]
        [StringLength(10)]
        [RegularExpression("^(PDF|IMG)$", ErrorMessage = "File type must be PDF or IMG.")]
        public string? FileType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "File size must be positive.")]
        public int? FileSize { get; set; }
    }
}
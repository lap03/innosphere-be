using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SocialLinkModels
{
    public class CreateSocialLinkModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Platform is required.")]
        [StringLength(50)]
        [RegularExpression("^(Facebook|LinkedIn|Twitter|Instagram)$", ErrorMessage = "Invalid social media platform.")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "URL is required.")]
        [StringLength(512, ErrorMessage = "URL cannot exceed 512 characters.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Url { get; set; }
    }
}

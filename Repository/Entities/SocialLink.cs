using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class SocialLink : BaseEntity
    {
        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Platform is required.")]
        [StringLength(50)]
        [RegularExpression("^(Facebook|LinkedIn|Twitter|Instagram)$", ErrorMessage = "Invalid social media platform.")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "URL is required.")]
        [StringLength(512, ErrorMessage = "URL cannot exceed 512 characters.")]
        public string Url { get; set; }

        public virtual User User { get; set; }
    }
}

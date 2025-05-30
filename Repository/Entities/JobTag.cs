using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class JobTag : BaseEntity
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(100, ErrorMessage = "Tag name cannot exceed 100 characters.")]
        public string TagName { get; set; }

        public virtual ICollection<JobPostingTag> JobPostingTags { get; set; }
    }
}

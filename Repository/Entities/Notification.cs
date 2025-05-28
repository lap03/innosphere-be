using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Notification : BaseEntity
    {
        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(255, ErrorMessage = "Message cannot exceed 255 characters.")]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public virtual User User { get; set; }
    }
}
